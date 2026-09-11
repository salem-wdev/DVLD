using System.Data;
using DVLD.Core.Entities;

namespace DVLD.Core.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonInfoByIDAsync(int? PersonID);

        Task<Person> GetPersonInfoByNationalNoAsync(string NationalNo);

        Task<int> AddNewPersonAsync(Person person);

        Task<bool> UpdatePersonAsync(Person person);

        Task<bool> IsPersonExistsAsync(int? PersonID);

        Task<bool> IsPersonExistsAsync(string NationalNo);

        Task<bool> IsNationalNoUsedAsync(int? PersonID, string NationalNo);

        Task<bool> DeletePersonAsync(int? PersonID);

        Task<DataTable> GetAllPeopleAsync();

        Task<bool> HasPeopleAsync();
    }
}
