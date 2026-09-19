using DVLD.Domain.Common;
using DVLD.Domain.Enums;
using System.Net.Mail;
using System.Text.RegularExpressions;


namespace DVLD.Domain.Entities
{
    public class Person
    {
        // High-performance compiled regex for initial structural syntax validation
        private static readonly Regex EmailFormatRegex = new(
            @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

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

            var emailvalidation = ValidateEmail(Email);

            if (emailvalidation.IsFailure)
            {
                return Result.Failure(emailvalidation.Error);
            }

            if (!Phone.All(char.IsDigit))
                return Result.Failure(SharedErrors.InvalidInput<Person>("Invalid Phone Number, most be Numbers only"));

            return Result.Success();
        }
       
        public static Result<Person> Create(string nationalNo, string firstName, string secondName,
            string? thirdName, string lastName, DateTime dateOfBirth, GenderType gender, string address, string phone,
            string? email, int nationalityCountryID, string? imagePath)
        {
            if (email != null)
                email = email.Trim();

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

        private static Result ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure(SharedErrors.InvalidInput<Person>("Email cannot be empty."));

            string trimmedEmail = email.Trim();

            // 1. Length constraint according to RFC 5321 / RFC 5322
            if (trimmedEmail.Length > 254)
                return Result.Failure(SharedErrors.InvalidInput<Person>("Email exceeds the maximum allowed length of 254 characters."));

            // 2. Structural pattern validation (ensures valid characters, single '@', and domain separator)
            if (!EmailFormatRegex.IsMatch(trimmedEmail))
                return Result.Failure(SharedErrors.InvalidInput<Person>("Email format is invalid."));

            // 3. Strict RFC parsing and domain structure validation via BCL MailAddress
            try
            {
                var mailAddress = new MailAddress(trimmedEmail);

                // Ensure the parsed address matches the input exactly (prevents display name exploits like 'Name <email@domain.com>')
                if (mailAddress.Address != trimmedEmail)
                    return Result.Failure(SharedErrors.InvalidInput<Person>("Email format is invalid."));

                // Ensure the domain part contains a top-level domain separator (rejects local addresses like 'user@localhost')
                if (!mailAddress.Host.Contains('.'))
                    return Result.Failure(SharedErrors.InvalidInput<Person>("Email must contain a valid domain name."));
            }
            catch (FormatException)
            {
                return Result.Failure(SharedErrors.InvalidInput<Person>("Email format is invalid."));
            }

            return Result.Success();
        }
    }
}
