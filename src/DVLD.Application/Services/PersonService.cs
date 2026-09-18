using DVLD.Domain.Common;
using DVLD.Application.DTOs.People;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using DVLD.Application.Interfaces;
using System;

namespace DVLD.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository personRepository;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IFileStorageService fileStorageService;

        public PersonService(IPersonRepository personRepository, IDateTimeProvider dateTimeProvider, IFileStorageService fileStorageService)
        {
            this.personRepository = personRepository ??
                throw new ArgumentNullException(nameof(personRepository));

            this.dateTimeProvider = dateTimeProvider ??
                throw new ArgumentNullException(nameof(dateTimeProvider));

            this.fileStorageService = fileStorageService ??
                throw new ArgumentNullException(nameof(fileStorageService));
        }

        public async Task<Result<PersonResponseDTO>> AddNewAsync(AddPersonDTO personDTO)
        {
            if (personDTO == null)
                return Result<PersonResponseDTO>.Failure(SharedErrors.InvalidInput<Person>("PersonResponseDTO is empty"));

            // Check if the date of birth is in the future
            if (personDTO.DateOfBirth >dateTimeProvider.UtcNow)
                return Result<PersonResponseDTO>.Failure(SharedErrors.InvalidInput<Person>("Date of birth cannot be in the future."));

            // Check if the national number is already used by another person
            if (await personRepository.IsNationalNoUsedAsync(personDTO.NationalNo))
            {
                return Result<PersonResponseDTO>.Failure(SharedErrors.InvalidInput<Person>("National number is already used"));
            }

            string? newImagePath = null;

            // If the image path is not empty, check if the file exists and copy it to the storage folder
            if (!string.IsNullOrWhiteSpace(personDTO.ImagePath))
            {
                // Check if the image file exists
                if (fileStorageService.IsFileExists(personDTO.ImagePath))
                {
                    // Copy the image file to the storage folder
                    var copyResult = await fileStorageService.CopyFileToDestinationFolderWithGUIDAsync(personDTO.ImagePath, StorageFolder.People);
                    if (copyResult.IsFailure)
                        return Result<PersonResponseDTO>.Failure(copyResult.Error);

                    // Set the new image path to the copied file path
                    newImagePath = copyResult.Value;
                }
                else
                    return Result<PersonResponseDTO>.Failure(SharedErrors.NotFound<Person>("Image file does not exist"));
            }

            Result<Person> person = Person.Create(
                personDTO.NationalNo,
                personDTO.FirstName,
                personDTO.SecondName,
                personDTO.ThirdName,
                personDTO.LastName,
                personDTO.DateOfBirth,
                personDTO.Gender,
                personDTO.Address,
                personDTO.Phone,
                personDTO.Email,
                personDTO.NationalityCountryID,
                newImagePath // Set the new image path to the copied file path
                );

            // If creation was faid delete coped image 
            if (person.IsFailure)
            {
                // 
                if (newImagePath != null)
                    fileStorageService.DeleteFile(newImagePath);

                return Result<PersonResponseDTO>.Failure(person.Error);
            }

            // Add the new person to the repository
            var newPersonID = await personRepository.AddNewAsync(person.Value);

            // if adding the new person to the repository was failed delete the copied image file
            if (newPersonID.IsFailure)
            {
                if (newImagePath != null)
                    fileStorageService.DeleteFile(newImagePath);
                
                return Result<PersonResponseDTO>.Failure(newPersonID.Error);
            }

            // return the new person details as a PersonResponseDTO
            return Result<PersonResponseDTO>.Success(new PersonResponseDTO
            (
                newPersonID.Value,
                personDTO.NationalNo,
                personDTO.FirstName,
                personDTO.SecondName,
                personDTO.ThirdName,
                personDTO.LastName,
                personDTO.DateOfBirth,
                personDTO.Gender,
                personDTO.Address,
                personDTO.Phone,
                personDTO.Email,
                personDTO.NationalityCountryID,
                newImagePath
            ));

        }

        public async Task<Result<PersonResponseDTO>> UpdateAsync(UpdatePersonDTO personDTO)
        {
            bool isImageUpdated = false;

            if (personDTO == null)
                return Result<PersonResponseDTO>.Failure(SharedErrors.InvalidInput<Person>("PersonResponseDTO is empty"));

            // Check if the date of birth is in the future
            if (personDTO.DateOfBirth > dateTimeProvider.UtcNow)
                return Result<PersonResponseDTO>.Failure(SharedErrors.InvalidInput<Person>("Date of birth cannot be in the future."));

            // Check if the national number is already used by another person
            var person = await personRepository.GetByIDAsync(personDTO.PersonID);

            // Check if the person exists
            if (person.IsFailure)
                return Result<PersonResponseDTO>.Failure(person.Error);

            // Set old image path to newImagePath variable, so we do not loss the old image path if the new image path is not provided in the DTO
            string? newImagePath = person.Value.ImagePath;

            
            // If the new image path is not empty and the old image path is not empty and they are different, copy the new image file to the storage folder and delete the old image file
            if (!string.IsNullOrWhiteSpace(personDTO.ImagePath) && personDTO.ImagePath != person.Value.ImagePath)
            {
                // Check if the new image file exists
                if (fileStorageService.IsFileExists(personDTO.ImagePath))
                {
                    // Copy the new image file to the storage folder
                    var copyResult = await fileStorageService.CopyFileToDestinationFolderWithGUIDAsync(personDTO.ImagePath, StorageFolder.People);
                    if (copyResult.IsFailure)
                        return Result<PersonResponseDTO>.Failure(copyResult.Error);

                    newImagePath = copyResult.Value;
                    isImageUpdated = true;
                }
                else
                    return Result<PersonResponseDTO>.Failure(SharedErrors.NotFound<Person>("Image file does not exist"));
            }

            string oldImage = person.Value.ImagePath;

            // Update the person entity with the new details
            var result = person.Value.UpdateDetails(
                personDTO.FirstName,
                personDTO.SecondName,
                personDTO.ThirdName,
                personDTO.LastName,
                personDTO.DateOfBirth,
                personDTO.Gender,
                personDTO.Address,
                personDTO.Phone,
                personDTO.Email,
                personDTO.NationalityCountryID,
                personDTO.RemoveImage? "": newImagePath
                );

            // Check if the update was successful
            if (result.IsFailure)
            {
                if (isImageUpdated && !string.IsNullOrWhiteSpace(newImagePath))
                {
                    fileStorageService.DeleteFile(newImagePath);
                }
                return Result<PersonResponseDTO>.Failure(result.Error);
            }
            // Update the person entity in the database
            var updateResult = await personRepository.UpdateAsync(person.Value);


            // Check if the update was successful in database
            if (updateResult.IsFailure)
            {
                if(isImageUpdated && !string.IsNullOrWhiteSpace(newImagePath))
                {
                    fileStorageService.DeleteFile(newImagePath);
                }
                return Result<PersonResponseDTO>.Failure(updateResult.Error);
            }
            else
            {
                if ((isImageUpdated || personDTO.RemoveImage) && !string.IsNullOrWhiteSpace(oldImage))
                    fileStorageService.DeleteFile(oldImage); // Delete old image if adding was ssucceful
            }
            // Return the updated person details as a PersonResponseDTO
            return Result<PersonResponseDTO>.Success(new PersonResponseDTO
            (
                person.Value.PersonID.Value,
                person.Value.NationalNo,
                person.Value.FirstName,
                person.Value.SecondName,
                person.Value.ThirdName,
                person.Value.LastName,
                person.Value.DateOfBirth,
                person.Value.Gender,
                person.Value.Address,
                person.Value.Phone,
                person.Value.Email,
                person.Value.NationalityCountryID,
                person.Value.ImagePath
            ));
        }

        public async Task<Result> DeleteAsync(int personID)
        {
            var responedPerson = await FindAsync(personID);

            if (responedPerson.IsFailure)
                return Result.Failure(responedPerson.Error);

            var deleteResult = await personRepository.DeleteAsync(personID);
            if (deleteResult.IsFailure)
                return Result.Failure(deleteResult.Error);

            if (!string.IsNullOrWhiteSpace(responedPerson.Value.ImagePath))
            {
                fileStorageService.DeleteFile(responedPerson.Value.ImagePath);
            }
            return Result.Success();
        }

        public async Task<Result<PersonResponseDTO>> FindAsync(int personID)
        {
            if (personID <= 0)
                return Result<PersonResponseDTO>.Failure(SharedErrors.InvalidInput<Person>("Invalid person ID"));

            var personInfo = await personRepository.GetByIDAsync(personID);

            if (personInfo.IsFailure)
            {
                return Result<PersonResponseDTO>.Failure(personInfo.Error);
            }

            return Result<PersonResponseDTO>.Success(new PersonResponseDTO
            (
                personInfo.Value.PersonID.Value,
                personInfo.Value.NationalNo,
                personInfo.Value.FirstName,
                personInfo.Value.SecondName,
                personInfo.Value.ThirdName,
                personInfo.Value.LastName,
                personInfo.Value.DateOfBirth,
                personInfo.Value.Gender,
                personInfo.Value.Address,
                personInfo.Value.Phone,
                personInfo.Value.Email,
                personInfo.Value.NationalityCountryID,
                personInfo.Value.ImagePath
                ));
        }

        public async Task<Result<PersonResponseDTO>> FindAsync(string nationalNo)
        {
            var validationResult = IsNationalNoValid(nationalNo);

            if (validationResult.IsFailure)
            {
                return Result<PersonResponseDTO>.Failure(validationResult.Error);
            }

            var personInfo = await personRepository.GetByNationalNoAsync(nationalNo);

            if (personInfo.IsFailure)
            {
                return Result<PersonResponseDTO>.Failure(personInfo.Error);
            }

            return Result<PersonResponseDTO>.Success(new PersonResponseDTO
            (
                personInfo.Value.PersonID.Value,
                personInfo.Value.NationalNo,
                personInfo.Value.FirstName,
                personInfo.Value.SecondName,
                personInfo.Value.ThirdName,
                personInfo.Value.LastName,
                personInfo.Value.DateOfBirth,
                personInfo.Value.Gender,
                personInfo.Value.Address,
                personInfo.Value.Phone,
                personInfo.Value.Email,
                personInfo.Value.NationalityCountryID,
                personInfo.Value.ImagePath
                ));
        }

        public async Task<bool> IsExistsAsync(int personID)
        {
            if (personID <= 0)
                return false;

            return await personRepository.IsExistsAsync(personID);
        }

        public async Task<bool> IsExistsAsync(string nationalNo)
        {
            var validationResult = IsNationalNoValid(nationalNo);

            if (validationResult.IsFailure)
            {
                return false;
            }

            return await personRepository.IsExistsAsync(nationalNo);
        }

        public async Task<bool> IsNationalNoUsedAsync(string nationalNo, int? excludepersonId = null)
        {
            var validationResult = IsNationalNoValid(nationalNo);

            if (validationResult.IsFailure)
            {
                return false;
            }

            return await personRepository.IsNationalNoUsedAsync(nationalNo, excludepersonId);
        }

        public async Task<Result<IEnumerable<PersonResponseDTO>>> GetAllAsync()
        {
            var people = await personRepository.GetAllAsync();

            if (people.IsFailure)
            {
                return Result<IEnumerable<PersonResponseDTO>>.Failure(people.Error);
            }

            var peopleDto = people.Value.Select(person => new PersonResponseDTO
            (
                person.PersonID.Value,
                person.NationalNo,
                person.FirstName,
                person.SecondName,
                person.ThirdName,
                person.LastName,
                person.DateOfBirth,
                person.Gender,
                person.Address,
                person.Phone,
                person.Email,
                person.NationalityCountryID,
                person.ImagePath
            )).ToList();

            return Result<IEnumerable<PersonResponseDTO>>.Success(peopleDto);
        }

        public async Task<bool> HasPeopleAsync()
        {
            return await personRepository.HasPeopleAsync();
        }

        private Result IsNationalNoValid(string nationalNo)
        {
            if (nationalNo != null)
            {
                if (string.IsNullOrWhiteSpace(nationalNo))
                    return Result.Failure(SharedErrors.InvalidInput<Person>("The field 'NationalNo' is required and cannot be empty."));
                if (nationalNo.Length != 14 || !nationalNo.All(char.IsDigit))
                    return Result.Failure(SharedErrors.InvalidInput<Person>("Invalid national number. It must be a 14-digit number."));
            }
            return Result.Success();
        }

    }
}
