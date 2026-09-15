using DVLD.Application.DTOs;
using DVLD.Application.Interfaces;

namespace DVLD.Application.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;
    public CountryService(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesAsync()
    {
        var country = await _countryRepository.GetAllAsync();

        List<CountryDTO> countryDTOs = new List<CountryDTO>();
        foreach (var c in country)
        {
            countryDTOs.Add(new CountryDTO
            {
                CountryID = c.CountryID,
                CountryName = c.CountryName
            });
        }

        return countryDTOs;
    }

    public async Task<CountryDTO?> GetCountryByIDAsync(int countryID)
    {
       var country = await _countryRepository.GetByIDAsync(countryID);

       if (country == null) return null;

       return new CountryDTO
       {
           CountryID = country.CountryID,
           CountryName = country.CountryName
       };
    }

    public async Task<CountryDTO?> GetCountryByNameAsync(string countryName)
    {
        var country = await _countryRepository.GetByNameAsync(countryName);

        if (country == null) return null;

        return new CountryDTO
        {
            CountryID = country.CountryID,
            CountryName = country.CountryName
        };
    }
}