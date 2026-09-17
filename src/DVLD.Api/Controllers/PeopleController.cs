using DVLD.Application.DTOs.People;
using DVLD.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PeopleController(IPersonService personService)
        {
            _personService = personService;
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("ID must be greater than 0.");

            var result = await _personService.FindAsync(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("by-national-no/{nationalNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByNationalNo(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
                return BadRequest("National number is required.");

            var result = await _personService.FindAsync(nationalNo);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PersonResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(AddPersonDTO dto)
        {
            var result = await _personService.AddNewAsync(dto);

            if (result.IsFailure)
                return Conflict(result.Error);

            return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.Value.PersonID },
                    result.Value
                );
        }

        [HttpPut("{id:int}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonDTO dto)
        {
            if (id <= 0 || id != dto.PersonID)
            {
                return BadRequest("Route ID does not match DTO ID.");
            }

            var result = await _personService.UpdateAsync(dto);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PersonResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _personService.GetAllAsync();

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID must be greater than 0");

            var result = await _personService.DeleteAsync(id);

            if(result.IsFailure)
                return NotFound(result.Error);

            return NoContent();
        }

        [HttpGet("is-national-no-used/{nationalNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsNationalNoUsed(string nationalNo, [FromQuery] int? personId = null)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
                return BadRequest("National number is required.");

            var result = await _personService.IsNationalNoUsedAsync(nationalNo, personId);

            return Ok(result);
        }
    }
}
