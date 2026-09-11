using DVLD.Core.DTOs.People;

namespace DVLD.Core.Interfaces
{
    public interface IPersonService
    {
        Task<PersonResponseDTO?> AddNewAsync(AddPersonRequestDTO person);

        Task<PersonResponseDTO?> UpdateAsync(PersonRequestDTO person);

        Task<bool> DeleteAsync(int personID);

        Task<PersonResponseDTO?> FindAsync(int personID);

        Task<PersonResponseDTO?> FindAsync(string nationalNo);

        Task<bool> IsExistsAsync(int personID);

        Task<bool> IsExistsAsync(string nationalNo);

        Task<bool> IsNationalNoUsedAsync(int personID, string nationalNo);

        Task<IEnumerable<PersonResponseDTO>> GetAllAsync();

        Task<bool> HasPeopleAsync();
    }
}
