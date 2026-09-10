using Microsoft.AspNetCore.Mvc;
using DVLD.Core.DTOs;
using DVLD.Core.Interfaces;

namespace DVLD.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ICountryService _countryService;

    // حقن التبعية عبر المشيد
    public CountriesController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CountryDTO>))]
    public async Task<ActionResult<IEnumerable<CountryDTO>>> GetAllCountries()
    {
        var countries = await _countryService.GetAllCountriesAsync();
        return Ok(countries);
    }

    [HttpGet("{id:int}", Name = "GetCountryByID")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CountryDTO>> GetCountryByID(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid country ID.");
        }

        var country = await _countryService.GetCountryByIDAsync(id);

        if (country == null)
        {
            return NotFound($"Country with ID {id} not found.");
        }

        return Ok(country);
    }

    [HttpGet("by-name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CountryDTO>> GetCountryByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Country name cannot be empty.");
        }

        var country = await _countryService.GetCountryByNameAsync(name);

        if (country == null)
        {
            return NotFound($"Country with name '{name}' not found.");
        }

        return Ok(country);
    }
}