using DVLD.Core.DTOs.People;
using DVLD.Core.Common;

namespace DVLD.Core.Interfaces
{
    public interface IPersonService
    {
        Task<Result<PersonResponseDTO>> AddNewAsync(AddPersonDTO person);
        Task<Result<PersonResponseDTO>> UpdateAsync(UpdatePersonDTO person);
        Task<Result> DeleteAsync(int personID);
        Task<Result<PersonResponseDTO>> FindAsync(int personID);
        Task<Result<PersonResponseDTO>> FindAsync(string nationalNo);
        Task<bool> IsExistsAsync(int personID);
        Task<bool> IsExistsAsync(string nationalNo);
        Task<bool> IsNationalNoUsedAsync(string nationalNo, int? personID = null);
        Task<Result<IEnumerable<PersonResponseDTO>>> GetAllAsync();
        Task<bool> HasPeopleAsync();
    }
}
