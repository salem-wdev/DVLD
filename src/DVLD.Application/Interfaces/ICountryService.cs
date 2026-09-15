using DVLD.Application.DTOs;

namespace DVLD.Application.Interfaces;

public interface ICountryService
{
    Task<IEnumerable<CountryDTO>> GetAllCountriesAsync();
    Task<CountryDTO?> GetCountryByIDAsync(int countryID);
    Task<CountryDTO?> GetCountryByNameAsync(string countryName);
}