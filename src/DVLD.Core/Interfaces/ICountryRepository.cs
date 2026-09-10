using DVLD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Core.Interfaces
{
    public interface ICountryRepository
    {
        Task<Country?> GetByIDAsync(int countryID);
        Task<Country?> GetByNameAsync(string countryName);
        Task<IEnumerable<Country>> GetAllAsync();
    }
}
