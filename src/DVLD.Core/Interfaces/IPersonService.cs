using DVLD.Core.Entities;
using DVLD.Core.Enums;
using System.Data;

namespace DVLD.Core.Interfaces
{
    public interface IPersonService
    {
        Task<bool> AddNewAsync();

        Task<bool> UpdateAsync();

        Task<bool> DeleteAsync(int? PersonID);

        Task<Person> FindAsync(int? PersonID);

        Task<Person> FindAsync(string NationalNo);

        Task<bool> IsExistsAsync(int? PersonID);

        Task<bool> IsExistsAsync(string NationalNo);

        Task<bool> IsNationalNoUsedAsync(int? PersonID, string NationalNo);

        Task<IEnumerable<Person>> GetAllAsync();

        Task<bool> HasPeopleAsync();

        Task<Person> CreateNewPersonAsync(Person person);
    }
}
