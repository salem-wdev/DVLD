using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace DVLD.UnitTests.Domain
{
    public class PersonTests
    {
        // ==========================================
        // 1. Happy Path
        // ==========================================

        [Fact]
        public void Create_WithValidData_ShouldReturnSuccessResult()
        {
            // Arrange
            var nationalNo = "12345678901234";
            var firstName = "Salem";
            var secondName = "Ahmed";
            var thirdName = "Ali";
            var lastName = "Habtor";
            var dateOfBirth = new DateTime(1995, 1, 1);
            var gender = GenderType.Male;
            var address = "Main Street";
            var phone = "777123456";
            var email = "test@example.com";
            var countryId = 1;
            string? imagePath = null;

            // Act
            var result = Person.Create(
                nationalNo, firstName, secondName, thirdName, lastName,
                dateOfBirth, gender, address, phone, email, countryId, imagePath);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.NationalNo.Should().Be(nationalNo);
            result.Value.FullName.Should().Be("Salem Ahmed Ali Habtor");
        }

        // ==========================================
        // 2. Failure Scenarios / Edge Cases
        // ==========================================

        [Theory]
        [InlineData("")]                         // Empty string
        [InlineData("   ")]                      // Whitespace only
        [InlineData("123")]                      // Less than 14 digits
        [InlineData("123456789012345")]          // More than 14 digits
        [InlineData("1234567890123A")]          // 14 characters, but contains letters
        public void Create_WithInvalidNationalNo_ShouldReturnFailure(string invalidNationalNo)
        {
            // Arrange
            var dateOfBirth = new DateTime(1995, 1, 1);

            // Act
            var result = Person.Create(
                invalidNationalNo, "Salem", "Ahmed", null, "Habtor",
                dateOfBirth, GenderType.Male, "Main Street", "777123456", null, 1, null);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Person.InvalidInput");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidFirstName_ShouldReturnFailure(string invalidFirstName)
        {
            // Arrange
            var dateOfBirth = new DateTime(1995, 1, 1);

            // Act
            var result = Person.Create(
                "12345678901234", invalidFirstName, "Ahmed", null, "Habtor",
                dateOfBirth, GenderType.Male, "Main Street", "777123456", null, 1, null);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Person.InvalidInput");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidLastName_ShouldReturnFailure(string invalidLastName)
        {
            // Arrange
            var dateOfBirth = new DateTime(1995, 1, 1);

            // Act
            var result = Person.Create(
                "12345678901234", "Salem", "Ahmed", null, invalidLastName,
                dateOfBirth, GenderType.Male, "Main Street", "777123456", null, 1, null);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Person.InvalidInput");
        }

        //[Fact]
        //public void Create_WithFutureDateOfBirth_ShouldReturnFailure()
        //{
        //    // Arrange
        //    var futureDateOfBirth = DateTime.Now.AddDays(1); // Future date

        //    // Act
        //    var result = Person.Create(
        //        "12345678901234", "Salem", "Ahmed", null, "Habtor",
        //        futureDateOfBirth, GenderType.Male, "Main Street", "777123456", null, 1, null);

        //    // Assert
        //    result.IsFailure.Should().BeTrue();
        //    result.Error.Code.Should().Be("Person.InvalidInput");
        //}

        [Theory]
        [InlineData("plainaddress")]             // Missing @ and domain
        [InlineData("@missingusername.com")]     // Missing username
        [InlineData("missingdomain@.com")]       // Missing domain name
        public void Create_WithInvalidEmailFormat_ShouldReturnFailure(string invalidEmail)
        {
            // Arrange
            var dateOfBirth = new DateTime(1995, 1, 1);

            // Act
            var result = Person.Create(
                "12345678901234", "Salem", "Ahmed", null, "Habtor",
                dateOfBirth, GenderType.Male, "Main Street", "777123456", invalidEmail, 1, null);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Person.InvalidInput");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("8751d4d5")]
        public void Create_WithInvalidPhone_ShouldReturnFailure(string invalidPhone)
        {
            // Arrange
            var dateOfBirth = new DateTime(1995, 1, 1);

            // Act
            var result = Person.Create(
                "12345678901234", "Salem", "Ahmed", null, "Habtor",
                dateOfBirth, GenderType.Male, "Main Street", invalidPhone, null, 1, null);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Person.InvalidInput");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithEmptyAddress_ShouldReturnFailure(string invalidAddress)
        {
            // Arrange
            var dateOfBirth = new DateTime(1995, 1, 1);

            // Act
            var result = Person.Create(
                "12345678901234", "Salem", "Ahmed", null, "Habtor",
                dateOfBirth, GenderType.Male, invalidAddress, "777123456", null, 1, null);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Person.InvalidInput");
        }
    }
}