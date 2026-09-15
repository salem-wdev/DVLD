using DVLD.Domain.Entities;
using DVLD.Domain.Common;

namespace DVLD.Application.Interfaces
{
    public interface IPersonRepository
    {
        Task<Result<Person>> GetByIDAsync(int personId);
        Task<Result<Person>> GetByNationalNoAsync(string nationalNo);
        Task<Result<int>> AddNewAsync(Person person);
        Task<Result> UpdateAsync(Person person);
        Task<bool> IsExistsAsync(int personId);
        Task<bool> IsExistsAsync(string nationalNo);
        Task<bool> IsNationalNoUsedAsync(string nationalNo, int? excludepersonId = null);
        Task<Result> DeleteAsync(int personId);
        Task<Result<IEnumerable<Person>>> GetAllAsync();
        Task<bool> HasPeopleAsync();
    }
}
