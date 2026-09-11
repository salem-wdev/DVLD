using DVLD.Core.DTOs;

namespace DVLD.Core.Interfaces;

public interface ICountryService
{
    Task<IEnumerable<CountryDTO>> GetAllCountriesAsync();
    Task<CountryDTO?> GetCountryByIDAsync(int countryID);
    Task<CountryDTO?> GetCountryByNameAsync(string countryName);
}