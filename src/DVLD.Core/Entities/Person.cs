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

        public Person(int? personID, string nationalNo, string firstName, string secondName,
            string? thirdName, string lastName, DateTime dateOfBirth, GenderType gender, string address, string phone,
            string? email, int nationalityCountryID, string? imagePath, Country? country)
        {
            PersonID = personID;
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

    }
}
