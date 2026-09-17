using DVLD.Application.Interfaces;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using DVLD.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DVLD.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class responsible for data access operations related to the Person entity.
    /// Uses ADO.NET for high-performance database interactions.
    /// </summary>
    public class PersonRepository : IPersonRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(IConfiguration configuration, ILogger<PersonRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves a person record by its unique identifier.
        /// </summary>
        public async Task<Result<Person>> GetByIDAsync(int personId)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath FROM People WHERE PersonID = @PersonID;";

            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);

            if (await reader.ReadAsync())
            {
                return MapToPerson(reader);
            }

            return Result<Person>.Failure(SharedErrors.NotFound<Person>($"ID: {personId}"));
        }

        /// <summary>
        /// Retrieves a person record based on their unique National Number.
        /// </summary>
        public async Task<Result<Person>> GetByNationalNoAsync(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
                return Result<Person>.Failure(SharedErrors.InvalidInput<Person>($"National number cannot be empty or whitespace."));

            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath FROM People WHERE NationalNo = @NationalNo;";

            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);

            if (await reader.ReadAsync())
            {
                return MapToPerson(reader);
            }

            return Result<Person>.Failure(SharedErrors.NotFound<Person>($"NationalNo: {nationalNo}"));
        }

        /// <summary>
        /// Inserts a new person record into the database using a stored procedure.
        /// </summary>
        public async Task<Result<int>> AddNewAsync(Person person)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("SP_AddNewPerson", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = person.NationalNo;
            command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = person.FirstName;
            command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = person.SecondName;

            // Handle nullable fields using DBNull.Value
            command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20).Value = (object?)person.ThirdName ?? DBNull.Value;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = person.LastName;
            command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = person.DateOfBirth;
            command.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = person.Gender;
            command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = person.Address;
            command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = person.Phone;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = (object?)person.Email ?? DBNull.Value;
            command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = person.NationalityCountryID;
            command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 250).Value = (object?)person.ImagePath ?? DBNull.Value;

            // Define output parameter to capture the newly generated PersonID
            var outputIdParam = new SqlParameter("@NewPersonID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputIdParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            if (outputIdParam.Value != DBNull.Value)
            {
                return Result<int>.Success((int)outputIdParam.Value);
            }

            return Result<int>.Failure(SharedErrors.AddFailed<Person>("Database rejected the insert or returned no ID."));
        }

        /// <summary>
        /// Updates an existing person record in the database.
        /// </summary>
        public async Task<Result> UpdateAsync(Person person)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE People 
                             SET NationalNo = @NationalNo, FirstName = @FirstName, SecondName = @SecondName, 
                                 ThirdName = @ThirdName, LastName = @LastName, DateOfBirth = @DateOfBirth, 
                                 Gendor = @Gendor, Address = @Address, Phone = @Phone, Email = @Email, 
                                 NationalityCountryID = @NationalityCountryID, ImagePath = @ImagePath
                             WHERE PersonID = @PersonID;";

            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = person.PersonID;
            command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = person.NationalNo;
            command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = person.FirstName;
            command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = person.SecondName;

            // Handle nullable fields using DBNull.Value
            command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20).Value = (object?)person.ThirdName ?? DBNull.Value;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = person.LastName;
            command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = person.DateOfBirth;
            command.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = person.Gender;
            command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = person.Address;
            command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = person.Phone;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = (object?)person.Email ?? DBNull.Value;
            command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = person.NationalityCountryID;
            command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 250).Value = (object?)person.ImagePath ?? DBNull.Value;

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
                return Result.Success();

            return Result.Failure(SharedErrors.UpdateFailed<Person>($"ID {person.PersonID} was not found."));
        }

        /// <summary>
        /// Deletes a person record from the database based on their ID.
        /// </summary>
        public async Task<Result> DeleteAsync(int personId)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "DELETE FROM People WHERE PersonID = @PersonID;";

            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
                return Result.Success();

            return Result.Failure(SharedErrors.DeleteFailed<Person>($"ID {personId} was not found."));
        }

        /// <summary>
        /// Retrieves all people records from the database.
        /// </summary>
        public async Task<Result<IEnumerable<Person>>> GetAllAsync()
        {
            var people = new List<Person>();

            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath FROM People ORDER BY FirstName;";

            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return Result<IEnumerable<Person>>.Success(people);

            while (await reader.ReadAsync())
            {
                var person = MapToPerson(reader);

                if (person.IsSuccess)
                {
                    people.Add(person.Value);
                }
                else
                {
                    int personId = reader.GetInt32(reader.GetOrdinal("PersonID"));

                    _logger.LogWarning(
                        "Skipping corrupted person record. PersonID: {PersonID}, ErrorCode: {ErrorCode}, ErrorMessage: {ErrorMessage}",
                        personId,
                        person.Error?.Code,
                        person.Error?.Message
                    );
                }
            }

            return Result<IEnumerable<Person>>.Success(people);
        }

        /// <summary>
        /// Checks if a person exists in the database by their ID.
        /// </summary>
        public async Task<bool> IsExistsAsync(int personId)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT TOP 1 1 FROM People WHERE PersonID = @PersonID;";

            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;

            await connection.OpenAsync();
            return await command.ExecuteScalarAsync() != null;
        }

        /// <summary>
        /// Checks if a person exists in the database by their National Number.
        /// </summary>
        public async Task<bool> IsExistsAsync(string nationalNo)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT TOP 1 1 FROM People WHERE NationalNo = @NationalNo;";

            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;

            await connection.OpenAsync();
            return await command.ExecuteScalarAsync() != null;
        }

        /// <summary>
        /// Validates if a National Number is already assigned to a person.
        /// Allows excluding a specific PersonID (useful during update operations).
        /// </summary>
        public async Task<bool> IsNationalNoUsedAsync(string nationalNo, int? excludepersonId = null)
        {
            using var connection = new SqlConnection(_connectionString);

            // Build query dynamically to append exclusion condition if an ID is provided
            string query = "SELECT TOP 1 1 FROM People WHERE NationalNo = @NationalNo";
            if (excludepersonId.HasValue)
            {
                query += " AND PersonID != @ExcludePersonID";
            }

            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;

            if (excludepersonId.HasValue)
            {
                command.Parameters.Add("@ExcludePersonID", SqlDbType.Int).Value = excludepersonId.Value;
            }

            await connection.OpenAsync();
            return await command.ExecuteScalarAsync() != null;
        }

        /// <summary>
        /// Checks if the People table contains any records.
        /// </summary>
        public async Task<bool> HasPeopleAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT TOP 1 1 FROM People;";

            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            return await command.ExecuteScalarAsync() != null;
        }

        // ==========================================
        // Helper Methods
        // ==========================================

        /// <summary>
        /// Maps a SqlDataReader record to a Person entity to ensure clean mapping logic and avoid code duplication.
        /// Handles DBNull checks for nullable database columns.
        /// </summary>
        private Result<Person> MapToPerson(SqlDataReader reader)
        {
            var result = Person.Load(
                personID: reader.GetInt32(reader.GetOrdinal("PersonID")),
                nationalNo: reader.GetString(reader.GetOrdinal("NationalNo")),
                firstName: reader.GetString(reader.GetOrdinal("FirstName")),
                secondName: reader.GetString(reader.GetOrdinal("SecondName")),
                thirdName: reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ThirdName")),
                lastName: reader.GetString(reader.GetOrdinal("LastName")),
                dateOfBirth: reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                gender: (GenderType)reader.GetByte(reader.GetOrdinal("Gendor")),
                address: reader.GetString(reader.GetOrdinal("Address")),
                phone: reader.GetString(reader.GetOrdinal("Phone")),
                email: reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email")),
                nationalityCountryID: reader.GetInt32(reader.GetOrdinal("NationalityCountryID")),
                imagePath: reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? string.Empty : reader.GetString(reader.GetOrdinal("ImagePath"))
            );

            if (result.IsFailure)
                return Result<Person>.Failure(result.Error);

            return Result<Person>.Success(result.Value);
        }
    }
}