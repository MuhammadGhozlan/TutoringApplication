using Microsoft.AspNetCore.Mvc;
using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IRepositories;

namespace TutoringWebsite.API.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("add")]
        public async Task<ActionResult<StudentResponseDto>> AddStudent([FromBody] StudentRequestDto request)
        {
            if (request == null || String.IsNullOrEmpty(request.Email) || String.IsNullOrEmpty(request.Name))
            {
                return BadRequest("request is null");
            }
            var student = await _studentService.AddStudent(request);
            return Ok(student);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("remove")]
        public async Task<ActionResult<string>> RemoveStudent([FromBody] int id)
        {
            if (id == 0)
            {
                return BadRequest("request is null");
            }
            var student = await _studentService.RemoveStudent(id);
            return Ok(student);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("update")]
        public async Task<ActionResult<StudentResponseDto>> UpdateStudent([FromBody] StudentRequestDto request, int id)
        {
            if (id == 0 || request == null)
            {
                return BadRequest("request is null");
            }
            var student = await _studentService.UpdateStudent(id, request);
            return Ok(student);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("getStudent")]
        public async Task<ActionResult<StudentResponseDto>> GetStudent([FromBody] StudentRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("request is null");
            }
            var student = await _studentService.GetStudent(request);
            return Ok(student);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("getStudents")]
        public async Task<ActionResult<List<StudentResponseDto>>> GetStudents()
        {
            var students = await _studentService.GetStudents();
            if (students.Count == 0)
            {
                return BadRequest("no students were found");
            }
            return Ok(students);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("filterStudents")]
        public async Task<ActionResult<List<StudentResponseDto>>> FilterStudents([FromBody] StudentFilterRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("request is null");
            }
            var students = await _studentService.FilterStudents(request);
            if (students.Count == 0)
            {
                return BadRequest("no students were found");
            }
            return Ok(students);
        }
    }
}
