using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IRepositories;

namespace TutoringWebsite.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly ILogger<StudentService> _logger;
        private readonly IStudentRepository _studentRepo;

        public StudentService(ILogger<StudentService> logger,
                              IStudentRepository studentRepo)
        {
            _logger = logger;
            _studentRepo = studentRepo;
        }
        public async Task<StudentResponseDto> AddStudent(StudentRequestDto request)
        {          
            if(request == null || String.IsNullOrEmpty(request.Email) || String.IsNullOrEmpty(request.Name))
            {
                _logger.LogError("request cannot be null");
                throw new InvalidOperationException("request is null");
            }

            var student = await _studentRepo.AddStudent(request);

            return student;
        }

        public async Task<List<StudentResponseDto>> FilterStudents(StudentFilterRequestDto request)
        {
            if(request == null)
            {
                _logger.LogError("you have to choose a filter");
                throw new InvalidOperationException("you have to choose a filter");
            }
            var students = await _studentRepo.FilterStudents(request);
            if(students.Count == 0)
            {
                _logger.LogError("there are no students for this course");
                throw new InvalidOperationException("there are no students for this course");
            }
            return students;
        }

        public async Task<StudentResponseDto> GetStudent(StudentRequestDto request)
        {
            if(request == null)
            {
                _logger.LogError("you have to choose a filter");
                throw new InvalidOperationException("you have to choose a filter");
            }

            var student = await _studentRepo.GetStudent(request);
            if(student == null)
            {
                _logger.LogError("no student was found");
                throw new InvalidOperationException("no student was found");
            }
            return student;
        }

        public async Task<List<StudentResponseDto>> GetStudents()
        {
            var students = await _studentRepo.GetStudents();
            if(students.Count == 0)
            {
                _logger.LogError("no students were found");
                throw new InvalidOperationException("no students were found");
            }
            return students;
        }

        public async Task<string> RemoveStudent(int id)
        {
            if(id == 0)
            {
                _logger.LogError("you have to enter a valid Id");
                throw new InvalidOperationException("Id entered is not valid Id");
            }

            var student = await _studentRepo.RemoveStudent(id);

            return student;
        }

        public async Task<StudentResponseDto> UpdateStudent(int id, StudentRequestDto request)
        {
            if(request == null || id == 0)
            {
                _logger.LogError("either the id is not valid or the entered request is incorrect");
                throw new InvalidOperationException("either the id is not valid or the entered request is incorrect");
            }

            var student = await _studentRepo.UpdateStudent(id, request);
            if(student == null)
            {
                _logger.LogError("no student was updated, check the id or the request body");
                throw new InvalidOperationException("no student was updated, check the id or the request body");
            }
            return student;
        }
    }
}
