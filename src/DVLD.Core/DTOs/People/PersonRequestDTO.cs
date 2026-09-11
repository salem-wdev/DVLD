using System;
using System.Collections.Generic;
using System.Linq;
using DVLD.Core.Enums;
namespace DVLD.Core.DTOs.People
{
     public record PersonRequestDTO(

        int? PersonID,

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
    { }
}
