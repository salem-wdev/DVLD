using DVLD.Core.Common;
using DVLD.Core.Enums;


namespace DVLD.Core.Entities
{
    public class Person
    {
        public int? PersonID { get;}

        public string NationalNo { get; }
        public string FirstName { get;}
        public string SecondName { get;}
        public string? ThirdName { get;}
        public string LastName { get;}

        public string FullName =>
                string.Join(" ", new[] { FirstName, SecondName, ThirdName, LastName }
                    .Where(name => !string.IsNullOrWhiteSpace(name)));

        public DateTime DateOfBirth { get;}
        public GenderType Gender { get;}
        public string Address { get;}
        public string Phone { get;}
        public string? Email { get;}
        public int NationalityCountryID { get;}
        public string? ImagePath { get;}
        public Country? Country { get; }

        protected Person(string nationalNo, string firstName, string secondName,
            string? thirdName, string lastName, DateTime dateOfBirth, GenderType gender, string address, string phone,
            string? email, int nationalityCountryID, string? imagePath, Country? country)
        {
            NationalNo = nationalNo;
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
            Country = country;
        }

        protected Person(int PersonID, string nationalNo, string firstName, string secondName,
            string? thirdName, string lastName, DateTime dateOfBirth, GenderType gender, string address, string phone,
            string? email, int nationalityCountryID, string? imagePath, Country? country)
        {
            NationalNo = nationalNo;
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
            Country = country;
        }


        private static Result _IsValidInfo(string NationalNo, string FirstName, string SecondName,
             string LastName, DateTime DateOfBirth, string Address,
            string Phone, int NationalityCountryID, string? Email)
        {
            var fields = new (string Name, string Value)[]
            {
                (nameof(NationalNo), NationalNo),
                (nameof(FirstName), FirstName),
                (nameof(SecondName), SecondName),
                (nameof(LastName), LastName),
                (nameof(Address), Address),
                (nameof(Phone), Phone)
            };

            var emptyField = fields.FirstOrDefault(f => string.IsNullOrWhiteSpace(f.Value));

            if (emptyField.Name != null)
            {
                return Result<bool>.Failure($"The field '{emptyField.Name}' is required and cannot be empty.");
            }

            if (NationalityCountryID < 1)
            {
                return Result.Failure("Invalid nationality country ID.");
            }

            if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
            {
                return Result.Failure("Invalid email format.");
            }
            return Result.Success();
        }
       
        public static Result<Person?> Create(string nationalNo, string firstName, string secondName,
            string? thirdName, string lastName, DateTime dateOfBirth, GenderType gender, string address, string phone,
            string? email, int nationalityCountryID, string? imagePath, Country? country)
        {

            var validationResult = _IsValidInfo(nationalNo, firstName, secondName, lastName, dateOfBirth
                , address, phone, nationalityCountryID, email);
            if (validationResult.IsFailure)
            {
                return Result<Person?>.Failure("Invalid person information.");
            }

            return Result<Person?>.Success(new Person(nationalNo, firstName, secondName, thirdName, lastName, dateOfBirth
                , gender, address, phone, email, nationalityCountryID, imagePath, country));
        }

    }
}
