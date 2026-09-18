using DVLD.Domain.Common;
using DVLD.Domain.Enums;


namespace DVLD.Domain.Entities
{
    public class Person
    {
        public int? PersonID { get; private set; }

        public string NationalNo { get; private set; }
        public string FirstName { get; private set; }
        public string SecondName { get; private set; }
        public string? ThirdName { get; private set; }
        public string LastName { get; private set; }

        public string FullName =>
                string.Join(" ", new[] { FirstName, SecondName, ThirdName, LastName }
                    .Where(name => !string.IsNullOrWhiteSpace(name)));

        public DateTime DateOfBirth { get; private set; }
        public GenderType Gender { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }
        public string? Email { get; private set; }
        public int NationalityCountryID { get; private set; }
        public string? ImagePath { get; private set; }
        public Country? Country { get; internal set; }

        protected Person() { }

        private static Result _IsValidInfo(string? NationalNo, string FirstName, string SecondName,
             string LastName, DateTime DateOfBirth, string Address,
            string Phone, int NationalityCountryID, string? Email)
        {
            var fields = new (string Name, string Value)[]
            {
                (nameof(FirstName), FirstName),
                (nameof(SecondName), SecondName),
                (nameof(LastName), LastName),
                (nameof(Address), Address),
                (nameof(Phone), Phone)
            };

            var emptyField = fields.FirstOrDefault(f => string.IsNullOrWhiteSpace(f.Value));

            if (emptyField.Name != null)
            {
                return Result.Failure(SharedErrors.InvalidInput<Person>($"The field '{emptyField.Name}' is required and cannot be empty."));
            }

            if (NationalNo != null)
            {
                if (string.IsNullOrWhiteSpace(NationalNo))
                    return Result.Failure(SharedErrors.InvalidInput<Person>("The field 'NationalNo' is required and cannot be empty."));

                if (NationalNo.Length != 14 || !NationalNo.All(char.IsDigit))
                    return Result.Failure(SharedErrors.InvalidInput<Person>("Invalid national number. It must be a 14-digit number."));
            }

            if (NationalityCountryID < 1)
            {
                return Result.Failure(SharedErrors.InvalidInput<Person>("Invalid nationality country ID."));
            }

            if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
            {
                return Result.Failure(SharedErrors.InvalidInput<Person>("Invalid email format."));
            }

            if (!Phone.All(char.IsDigit))
                return Result.Failure(SharedErrors.InvalidInput<Person>("Invalid Phone Number, most be Numbers only"));

            return Result.Success();
        }
       
        public static Result<Person> Create(string nationalNo, string firstName, string secondName,
            string? thirdName, string lastName, DateTime dateOfBirth, GenderType gender, string address, string phone,
            string? email, int nationalityCountryID, string? imagePath)
        {

            var validationResult = _IsValidInfo(nationalNo, firstName, secondName, lastName, dateOfBirth
                , address, phone, nationalityCountryID, email);
            if (validationResult.IsFailure)
            {
                return Result<Person>.Failure(validationResult.Error);
            }

            return Result<Person>.Success(new Person{
                PersonID= null,
                NationalNo= nationalNo, 
                FirstName= firstName,
                SecondName = secondName,
                ThirdName = thirdName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Address = address,
                Phone = phone,
                Email = email,
                NationalityCountryID = nationalityCountryID,
                ImagePath = imagePath
            });
        }

        public static Result<Person> Load(int personID, string nationalNo, string firstName, string secondName,
    string? thirdName, string lastName, DateTime dateOfBirth, GenderType gender, string address, string phone,
    string? email, int nationalityCountryID, string? imagePath)
        {
            if (personID <= 0)
                return Result<Person>.Failure(SharedErrors.InvalidInput<Person>("Invalid Person ID."));

            var validationResult = _IsValidInfo(nationalNo, firstName, secondName, lastName, dateOfBirth
                , address, phone, nationalityCountryID, email);
            if (validationResult.IsFailure)
            {
                return Result<Person>.Failure(validationResult.Error);
            }

            return Result<Person>.Success(new Person
            {
                PersonID = personID,
                NationalNo = nationalNo,
                FirstName = firstName,
                SecondName = secondName,
                ThirdName = thirdName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Address = address,
                Phone = phone,
                Email = email,
                NationalityCountryID = nationalityCountryID,
                ImagePath = imagePath
            });
        }

        public Result UpdateDetails(
            string firstName,
            string secondName,
            string thirdName,
            string lastName,
            DateTime dateOfBirth,
            GenderType gender,
            string address,
            string phone,
            string? email,
            int nationalityCountryID,
            string? imagePath)
        {
            var validationResult = _IsValidInfo(null, firstName, secondName, lastName, dateOfBirth
               , address, phone, nationalityCountryID, email);
            if (validationResult.IsFailure)
            {
                return Result.Failure(validationResult.Error);
            }

            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            NationalityCountryID = nationalityCountryID;
            ImagePath = imagePath;

            return Result.Success();
        }
    }
}
