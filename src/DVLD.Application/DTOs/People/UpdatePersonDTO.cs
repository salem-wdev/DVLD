using DVLD.Domain.Enums;
namespace DVLD.Application.DTOs.People
{
     public record UpdatePersonDTO(

        int PersonID,

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
    { }
}
