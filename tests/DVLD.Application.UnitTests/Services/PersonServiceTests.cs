using DVLD.Application.DTOs.People;
using DVLD.Application.Interfaces;
using DVLD.Application.Services;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace DVLD.Application.UnitTests.Services
{
    public class PersonServiceTests
    {
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly PersonService _sut; // System Under Test

        public PersonServiceTests()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();

            // Setup default UtcNow to avoid future date issues
            _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(new DateTime(2026, 1, 1));

            _sut = new PersonService(
                _personRepositoryMock.Object,
                _dateTimeProviderMock.Object,
                _fileStorageServiceMock.Object);
        }

        #region Helper Methods to Generate Valid Data

        // Helper to create a valid AddPersonDTO passing Domain invariants
        private AddPersonDTO CreateValidAddPersonDTO(string? imagePath = null) =>
            new AddPersonDTO(
                NationalNo: "12345678901234",
                FirstName: "Salem",
                SecondName: "A",
                ThirdName: null,
                LastName: "Habtor",
                DateOfBirth: new DateTime(1995, 1, 1),
                Gender: GenderType.Male,
                Address: "Ataq, Shabwah",
                Phone: "777123456",
                Email: "salem@example.com",
                NationalityCountryID: 1,
                ImagePath: imagePath);

        // Helper to create a valid Person entity
        private Person CreateValidPerson(int id = 1, string? imagePath = null)
        {
            var personResult = Person.Create(
                "12345678901234", "Salem", "A", null, "Habtor",
                new DateTime(1995, 1, 1), GenderType.Male, "Ataq", "777123456",
                "salem@example.com", 1, imagePath);

            // Using reflection to set ID since it's private/protected init
            typeof(Person).GetProperty("PersonID")?.SetValue(personResult.Value, id);

            return personResult.Value;
        }

        #endregion

        #region Constructor Tests

        [Fact]
        public void Constructor_WithNullRepository_ShouldThrowArgumentNullException()
        {
            Action act = () => new PersonService(null!, _dateTimeProviderMock.Object, _fileStorageServiceMock.Object);
            act.Should().Throw<ArgumentNullException>().WithParameterName("personRepository");
        }

        #endregion

        #region AddNewAsync Tests

        [Fact]
        public async Task AddNewAsync_WithNullDTO_ShouldReturnFailure()
        {
            // Act
            var result = await _sut.AddNewAsync(null!);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Contain("InvalidInput");
        }

        [Fact]
        public async Task AddNewAsync_WithFutureDateOfBirth_ShouldReturnFailure()
        {
            // Arrange
            var dto = CreateValidAddPersonDTO();
            dto = dto with { DateOfBirth = new DateTime(2030, 1, 1) }; // Future date

            // Act
            var result = await _sut.AddNewAsync(dto);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Message.Should().Contain("Date of birth cannot be in the future.");
        }

        [Fact]
        public async Task AddNewAsync_WhenNationalNoExists_ShouldReturnFailure()
        {
            // Arrange
            var dto = CreateValidAddPersonDTO();
            _personRepositoryMock.Setup(repo => repo.IsNationalNoUsedAsync(dto.NationalNo, null))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.AddNewAsync(dto);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Message.Should().Contain("already used");
        }

        [Fact]
        public async Task AddNewAsync_WithValidDataAndNoImage_ShouldReturnSuccess()
        {
            // Arrange
            var dto = CreateValidAddPersonDTO();
            _personRepositoryMock.Setup(repo => repo.IsNationalNoUsedAsync(dto.NationalNo, null)).ReturnsAsync(false);

            _personRepositoryMock.Setup(repo => repo.AddNewAsync(It.IsAny<Person>())).ReturnsAsync(Result<int>.Success(1));

            // Act
            var result = await _sut.AddNewAsync(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.FirstName.Should().Be(dto.FirstName);
            _personRepositoryMock.Verify(repo => repo.AddNewAsync(It.IsAny<Person>()), Times.Once);
        }

        [Fact]
        public async Task AddNewAsync_WhenDatabaseFails_ShouldDeleteCopiedImageAndReturnFailure()
        {
            // Arrange
            var dto = CreateValidAddPersonDTO(imagePath: "temp/image.png");
            string copiedImagePath = "people/guid-image.png";

            _personRepositoryMock.Setup(repo => repo.IsNationalNoUsedAsync(dto.NationalNo, null)).ReturnsAsync(false);
            _fileStorageServiceMock.Setup(f => f.IsFileExists(dto.ImagePath)).Returns(true);
            _fileStorageServiceMock.Setup(f => f.CopyFileToDestinationFolderWithGUIDAsync(dto.ImagePath, StorageFolder.People))
                .ReturnsAsync(Result<string>.Success(copiedImagePath));

            // Simulating database failure
            _personRepositoryMock.Setup(repo => repo.AddNewAsync(It.IsAny<Person>()))
                        .ReturnsAsync(Result<int>.Failure(new DVLD.Domain.Common.Error("Database.Failure", "Failed to insert record into database.")));
            // Act
            var result = await _sut.AddNewAsync(dto);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Contain("Database.Failure");

            // Verify Rollback: Ensure the copied image was deleted
            _fileStorageServiceMock.Verify(f => f.DeleteFile(copiedImagePath), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        [Fact]
        public async Task UpdateAsync_WhenPersonNotFound_ShouldReturnFailure()
        {
            // Arrange
            var dto = new UpdatePersonDTO(99, "Salem", "A", null, "Habtor", new DateTime(1995, 1, 1), GenderType.Male, "Ataq", "777123456", "salem@ex.com", 1, null);

            _personRepositoryMock.Setup(repo => repo.GetByIDAsync(dto.PersonID))
                .ReturnsAsync(Result<Person>.Failure(SharedErrors.NotFound<Person>()));

            // Act
            var result = await _sut.UpdateAsync(dto);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Contain("NotFound");
        }

        [Fact]
        public async Task UpdateAsync_WithNewImage_ShouldCopyNewAndDeleteOldImageOnSuccess()
        {
            // Arrange
            string oldImagePath = "people/old-image.png";
            string tempNewImagePath = "temp/new-image.png";
            string copiedNewImagePath = "people/new-guid-image.png";

            var existingPerson = CreateValidPerson(1, oldImagePath);

            // Note: Added false at the end to represent the RemoveImage property we agreed upon
            var dto = new UpdatePersonDTO(
                1, "UpdatedName", "A", null, "Habtor",
                existingPerson.DateOfBirth, GenderType.Male,
                "Ataq", "777123456", "salem@ex.com", 1,
                tempNewImagePath, RemoveImage: false);

            // 1. Mock Repository Get
            _personRepositoryMock.Setup(repo => repo.GetByIDAsync(dto.PersonID))
                .ReturnsAsync(Result<Person>.Success(existingPerson));

            // 2. Mock File Storage (Copy logic)
            _fileStorageServiceMock.Setup(f => f.IsFileExists(tempNewImagePath))
                .Returns(true);
            _fileStorageServiceMock.Setup(f => f.CopyFileToDestinationFolderWithGUIDAsync(tempNewImagePath, StorageFolder.People))
                .ReturnsAsync(Result<string>.Success(copiedNewImagePath));

            // 3. Mock Repository Update
            _personRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Person>()))
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _sut.UpdateAsync(dto);

            // Assert - 1. Verify the returned response data
            result.IsSuccess.Should().BeTrue();
            result.Value.FirstName.Should().Be("UpdatedName");
            result.Value.ImagePath.Should().Be(copiedNewImagePath); // Ensure the returned DTO holds the new path

            // Assert - 2. Repository validation
            // Ensure we sent the entity to the database holding the new path and updates
            _personRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Person>(p =>
                p.ImagePath == copiedNewImagePath &&
                p.FirstName == "UpdatedName"
            )), Times.Once);

            // Assert - 3. File storage strict validation
            // A: Verify the old image was deleted (because the operation succeeded)
            _fileStorageServiceMock.Verify(f => f.DeleteFile(oldImagePath), Times.Once);

            // B: (Crucial negative verification) Ensure the system didn't delete the new image by mistake (verify no rollback occurred)
            _fileStorageServiceMock.Verify(f => f.DeleteFile(copiedNewImagePath), Times.Never);
        }

        #endregion

        #region DeleteAsync Tests

        [Fact]
        public async Task DeleteAsync_WhenPersonExists_ShouldDeleteFromDBAndRemoveImage()
        {
            // Arrange
            int personId = 1;
            string imagePath = "people/image.png";
            var existingPerson = CreateValidPerson(personId, imagePath);

            _personRepositoryMock.Setup(repo => repo.GetByIDAsync(personId))
                .ReturnsAsync(Result<Person>.Success(existingPerson));

            _personRepositoryMock.Setup(repo => repo.DeleteAsync(personId))
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _sut.DeleteAsync(personId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _personRepositoryMock.Verify(repo => repo.DeleteAsync(personId), Times.Once);
            _fileStorageServiceMock.Verify(f => f.DeleteFile(imagePath), Times.Once); // Ensures file deletion is called
        }

        [Fact]
        public async Task DeleteAsync_WhenDatabaseDeleteFails_ShouldNotDeleteImageAndReturnFailure()
        {
            // Arrange
            int personId = 1;
            string imagePath = "people/image.png";
            var existingPerson = CreateValidPerson(personId, imagePath);

            _personRepositoryMock.Setup(repo => repo.GetByIDAsync(personId))
                .ReturnsAsync(Result<Person>.Success(existingPerson));

            _personRepositoryMock.Setup(repo => repo.DeleteAsync(personId))
                .ReturnsAsync(Result.Failure(new DVLD.Domain.Common.Error("Database.Failure", "Cannot delete")));

            // Act
            var result = await _sut.DeleteAsync(personId);

            // Assert
            result.IsFailure.Should().BeTrue();
            _fileStorageServiceMock.Verify(f => f.DeleteFile(It.IsAny<string>()), Times.Never); // CRITICAL: Image must not be deleted if DB fails
        }

        #endregion

        #region FindAsync Tests

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task FindAsync_WithInvalidId_ShouldReturnFailure(int invalidId)
        {
            // Act
            var result = await _sut.FindAsync(invalidId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Contain("InvalidInput");
        }

        [Fact]
        public async Task FindAsync_WithValidId_ShouldReturnMappedDTO()
        {
            // Arrange
            int personId = 1;
            var existingPerson = CreateValidPerson(personId);

            _personRepositoryMock.Setup(repo => repo.GetByIDAsync(personId))
                .ReturnsAsync(Result<Person>.Success(existingPerson));

            // Act
            var result = await _sut.FindAsync(personId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.PersonID.Should().Be(personId);
            result.Value.NationalNo.Should().Be(existingPerson.NationalNo);
        }

        #endregion

        #region IsNationalNoValid / IsExists Tests

        [Theory]
        [InlineData("")]
        [InlineData("123")] // Too short
        [InlineData("1234567890123A")] // Contains letter
        public async Task IsExistsAsync_WithInvalidNationalNo_ShouldReturnFalse(string invalidNationalNo)
        {
            // Act
            var result = await _sut.IsExistsAsync(invalidNationalNo);

            // Assert
            result.Should().BeFalse();
            _personRepositoryMock.Verify(repo => repo.IsExistsAsync(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_WhenSuccessful_ShouldReturnListOfDTOs()
        {
            // Arrange
            var peopleList = new List<Person>
            {
                CreateValidPerson(1),
                CreateValidPerson(2)
            };

            _personRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result<IEnumerable<Person>>.Success(peopleList));

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);
            result.Value.First().PersonID.Should().Be(1);
        }

        #endregion
    }
}