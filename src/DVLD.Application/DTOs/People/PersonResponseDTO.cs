using DVLD.Domain.Enums;

namespace DVLD.Application.DTOs.People
{
    public record PersonResponseDTO(

        int PersonID,

        string NationalNo,
        string FirstName,
        string SecondName,
        string? ThirdName,
        string LastName,

        DateTime DateOfBirth,
        GenderType Gender,
        string Address,
        string Phone,
        string? Email,
        int NationalityCountryID,
        string? ImagePath
    )
    {
        public string FullName =>
               string.Join(" ", new[] { FirstName, SecondName, ThirdName, LastName }
                   .Where(name => !string.IsNullOrWhiteSpace(name)));
    }
}
