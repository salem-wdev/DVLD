using DVLD.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Core.DTOs.People
{
    public record AddPersonRequestDTO
        (
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
    }
}
