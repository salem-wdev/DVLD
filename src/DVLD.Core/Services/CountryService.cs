using DVLD.Core.DTOs;
using DVLD.Core.Interfaces;

namespace DVLD.Core.Services;

public class CountryService : ICountryService
{
    // قائمة تجريبية مؤقتة داخل الخدمة لعزل الكنترولر تماماً
    private static readonly List<CountryDTO> _countries = new()
    {
        new CountryDTO { CountryID = 1, CountryName = "Yemen" },
        new CountryDTO { CountryID = 2, CountryName = "Saudi Arabia" },
        new CountryDTO { CountryID = 3, CountryName = "Egypt" }
    };

    public Task<IEnumerable<CountryDTO>> GetAllCountriesAsync()
    {
        return Task.FromResult<IEnumerable<CountryDTO>>(_countries);
    }

    public Task<CountryDTO?> GetCountryByIDAsync(int countryID)
    {
        var country = _countries.FirstOrDefault(c => c.CountryID == countryID);
        return Task.FromResult(country);
    }

    public Task<CountryDTO?> GetCountryByNameAsync(string countryName)
    {
        var country = _countries.FirstOrDefault(c => c.CountryName.Equals(countryName, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(country);
    }
}