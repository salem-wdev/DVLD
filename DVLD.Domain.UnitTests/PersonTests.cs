using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using FluentAssertions;
using Xunit;
using DVLD.Domain.Common;

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
        // Empty, Null & Whitespaces
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]

        // Structural & Syntax Violations
        [InlineData("plainaddress")]
        [InlineData("@missingusername.com")]
        [InlineData("missingdomain@.com")]
        [InlineData("missingat.com")]
        [InlineData("user@domain@extra.com")]
        [InlineData("user@domain..com")]
        [InlineData("user@.domain.com")]
        [InlineData(".user@domain.com")]

        // Invalid Characters & Spaces
        [InlineData("user name@domain.com")]
        [InlineData("username@dom ain.com")]
        [InlineData("user<>@domain.com")]

        // Incomplete Domain / Localhost (No TLD)
        [InlineData("user@localhost")]
        [InlineData("user@domain")]

        // Display Name Exploit Attempts
        [InlineData("Display Name <user@domain.com>")]
        [InlineData("<user@domain.com>")]

        // RFC Limit Check (Exceeding 254 characters)
        [InlineData("verylongstringexceedingthemaximumallowablelengthaccordingtorfc5321standardsanddomainrulesappliedinsideourdomainentityvalidationlogictopreventbuffoverflowanddosattacksverylongstringexceedingthemaximumallowablelengthaccordingtorfc5321standardsanddomainrules@domain.com")]
        public void Create_WithInvalidEmail_ShouldReturnFailure(string? invalidEmail)
        {
            // Act
            var result = Person.Create(
                nationalNo: "12345678901234",
                firstName: "Salem",
                secondName: "A.",
                thirdName: null,
                lastName: "Habtor",
                dateOfBirth: new DateTime(1995, 1, 1),
                gender: GenderType.Male,
                address: "Ataq, Shabwah",
                phone: "777123456",
                email: invalidEmail,
                nationalityCountryID: 1,
                imagePath: null);

            // Assert
            result.IsFailure.Should().BeTrue($"Expected failure on input: '{invalidEmail}', but creation succeeded unexpectedly.");
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Should().NotBe(DVLD.Domain.Common.Error.None);
        }

        [Theory]
        // Standard Formats
        [InlineData("user@example.com")]
        [InlineData("firstname.lastname@domain.com")]

        // Plus Addressing / Sub-addressing / Tags
        [InlineData("user+newsletter@sub.example.com")]
        [InlineData("user#name@domain.com")]

        // Alphanumeric, Hyphens, and Dots
        [InlineData("user_name.123@domain-name.co.uk")]
        [InlineData("user-name@domain.edu")]
        [InlineData("first.middle.last@subdomain.domain.org")]

        // Numbered Domains and TLDs
        [InlineData("contact@company123.net")]
        [InlineData("support@domain.technology")]

        // Inputs with Surrounding Whitespaces (Verifies domain auto-trimming invariant)
        [InlineData("   valid.trimmed@example.com   ")]
        public void Create_WithValidEmail_ShouldReturnSuccess(string validEmail)
        {
            // Arrange: Valid baseline data for Person entity invariants
            const string nationalNo = "12345678901234";
            const string firstName = "Salem";
            const string secondName = "A.";
            const string? thirdName = null;
            const string lastName = "Habtor";
            var dateOfBirth = new DateTime(1995, 1, 1);
            const GenderType gender = GenderType.Male;
            const string address = "Ataq, Shabwah";
            const string phone = "777123456";
            const int nationalityCountryId = 1;
            const string? imagePath = null;

            // Act: Attempt to create Person with the candidate valid email
            var result = Person.Create(
                nationalNo: nationalNo,
                firstName: firstName,
                secondName: secondName,
                thirdName: thirdName,
                lastName: lastName,
                dateOfBirth: dateOfBirth,
                gender: gender,
                address: address,
                phone: phone,
                email: validEmail,
                nationalityCountryID: nationalityCountryId,
                imagePath: imagePath);

            // Assert: Verify creation succeeds and domain properties maintain validity
            result.IsSuccess.Should().BeTrue($"Failed on input: '{validEmail}' | Error: {result.Error?.Message ?? result.Error?.Code}");
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeNull();
            result.Value.Should().NotBeNull();
            result.Value.Email.Should().Be(validEmail.Trim());
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