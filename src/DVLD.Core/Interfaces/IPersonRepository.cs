using System.Data;
using DVLD.Core.Entities;

namespace DVLD.Core.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> GetByIDAsync(int? PersonID);

        Task<Person> GetByNationalNoAsync(string NationalNo);

        Task<int> AddNewAsync(Person person);

        Task<bool> UpdateAsync(Person person);

        Task<bool> IsExistsAsync(int? PersonID);

        Task<bool> IsExistsAsync(string NationalNo);

        Task<bool> IsNationalNoUsedAsync(int? PersonID, string NationalNo);

        Task<bool> DeleteAsync(int? PersonID);

        Task<IEnumerable<Person>> GetAllAsync();

        Task<bool> HasPeopleAsync();
    }
}
