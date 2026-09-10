using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DVLD.Core.Entities;
using DVLD.Core.Interfaces;

namespace DVLD.Infrastructure.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly string _connectionString;

    public CountryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<Country?> GetByIDAsync(int countryID)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SP_GetCountryByID", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add(new SqlParameter("@CountryID", SqlDbType.Int) { Value = countryID });

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Country(

                reader.GetInt32(reader.GetOrdinal("CountryID")),
                reader.GetString(reader.GetOrdinal("CountryName"))
                );
            
        }

        return null;
    }

    public async Task<Country?> GetByNameAsync(string countryName)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SP_GetCountryByName", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add(new SqlParameter("@CountryName", SqlDbType.NVarChar, 100) { Value = countryName });

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Country
            (
                reader.GetInt32(reader.GetOrdinal("CountryID")),
                reader.GetString(reader.GetOrdinal("CountryName"))
            );
        }

        return null;
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        var countries = new List<Country>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SP_GetAllCountries", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            countries.Add(new Country(
                reader.GetInt32(reader.GetOrdinal("CountryID")),
                reader.GetString(reader.GetOrdinal("CountryName"))
            ));
        }

        return countries;
    }
}