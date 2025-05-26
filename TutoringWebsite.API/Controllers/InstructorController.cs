using Microsoft.AspNetCore.Mvc;
using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IServices;

namespace TutoringWebsite.API.Controllers
{
    [ApiController]
    [Route("api/instructor")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("add")]
        public async Task<ActionResult<InstructorResponseDto>> AddInstructor([FromBody] InstructorRequestDto request)
        {
            if (request == null || String.IsNullOrEmpty(request.Email) || String.IsNullOrEmpty(request.Name) || request.DoB == null)
            {
                return BadRequest("request is null");
            }
            var instructor = await _instructorService.AddInstructor(request);
            return Ok(instructor);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("remove/{id}")]
        public async Task<ActionResult<string>> RemoveInstructor(int id)
        {
            if (id == 0)
            {
                return BadRequest("request is null");
            }
            var instructor = await _instructorService.RemoveInstructor(id);
            return Ok(instructor);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("update/{id}")]
        public async Task<ActionResult<InstructorResponseDto>> UpdateInstructor([FromBody] InstructorRequestDto request, int id)
        {
            if (id == 0 || request == null)
            {
                return BadRequest("request is null");
            }
            var instructor = await _instructorService.UpdateInstructor(id, request);
            return Ok(instructor);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("getInstructor")]
        public async Task<ActionResult<InstructorResponseDto>> GetInstructor([FromBody] InstructorRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("request is null");
            }
            var instructor = await _instructorService.GetInstructor(request);
            return Ok(instructor);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("getInstructors")]
        public async Task<ActionResult<List<InstructorResponseDto>>> GetInstructors()
        {
            var instructors = await _instructorService.GetInstructors();
            if (instructors.Count == 0)
            {
                return BadRequest("no instructors were found");
            }
            return Ok(instructors);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("filterInstructors")]
        public async Task<ActionResult<List<InstructorResponseDto>>> FilterInstructors([FromBody] InstructorFilterRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("request is null");
            }
            var instructors = await _instructorService.GetFilteredInstructors(request);
            if (instructors.Count == 0)
            {
                return BadRequest("no instructors were found");
            }
            return Ok(instructors);
        }
    }
}
