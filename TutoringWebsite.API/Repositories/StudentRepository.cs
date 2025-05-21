using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutoringWebsite.API.Data;
using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IRepositories;
using TutoringWebsite.API.Models;

namespace TutoringWebsite.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _dataContext;
        private readonly IdentityContext _identityContext;
        private readonly ILogger<StudentRepository> _logger;
        private readonly IMapper _mapper;

        public StudentRepository(DataContext dataContext,
                                 IdentityContext identityContext,
                                 ILogger<StudentRepository> logger,
                                 IMapper mapper)
        {
            _dataContext = dataContext;
            _identityContext = identityContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<StudentResponseDto> AddStudent(StudentRequestDto request)
        {
            if (request == null)
            {                
                _logger.LogError("request is null");
                throw new ArgumentNullException(nameof(request));
            }
            var user = await _identityContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                _logger.LogError("no user is registered for the same email");
                throw new ArgumentNullException(nameof(request));
            }
            if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                _logger.LogError("student has been disabled");
                throw new Exception("this account has been deleted or disabled");
            }
            Student student = new Student
            {
                Name = request.Name,
                Email = request.Email,
                UserId = user.Id,
                IsDeleted = false
            };
            await _dataContext.Students.AddAsync(student);
            await _dataContext.SaveChangesAsync();
            var response = _mapper.Map<StudentResponseDto>(student);
            return response;
        }

        public async Task<List<StudentResponseDto>> FilterStudents(StudentFilterRequestDto request)
        {
            if (request.CourseId != 0)
            {
                var studentIds = await _dataContext.Enrollments.Where(e => e.CourseId == request.CourseId).Select(e => e.StudentId).ToListAsync();
                if (!studentIds.Any())
                {
                    _logger.LogError("there are no students for this course");
                    throw new NullReferenceException("there are no students for this course");
                }
                var students = await _dataContext.Students.Where(s => studentIds.Contains(s.ID) && s.IsDeleted == false).ToListAsync();
                if (!students.Any())
                {
                    _logger.LogError("there are no students for this course");
                    throw new NullReferenceException("there are no students for this course");
                }
                return _mapper.Map<List<StudentResponseDto>>(students);
            }

            if (!String.IsNullOrWhiteSpace(request.CourseName))
            {
                var courses = await _dataContext.Courses.Where(c => c.Title == request.CourseName).Select(c => c.ID).ToListAsync();
                if (!courses.Any())
                {
                    _logger.LogError("there are no courses similar to this name");
                    throw new NullReferenceException("there are no courses similar to this name");
                }
                var studentsIds = await _dataContext.Enrollments.Where(e => courses.Contains(e.CourseId)).Select(e => e.StudentId).ToListAsync();
                var students = await _dataContext.Students.Where(s => studentsIds.Contains(s.ID) && s.IsDeleted == false).ToListAsync();
                if (!students.Any())
                {
                    _logger.LogError("there are no students for this course");
                    throw new NullReferenceException("there are no students for this course");
                }
                return _mapper.Map<List<StudentResponseDto>>(students);
            }
            if (request.InstructorId != 0)
            {
                var course = await _dataContext.Courses.Where(c => c.InstructorId == request.InstructorId).FirstOrDefaultAsync();
                if (course == null)
                {
                    _logger.LogError("there are no courses for this instructor");
                    throw new NullReferenceException("there are no courses for this instructor");
                }
                var studentIds = await _dataContext.Enrollments.Where(e => e.CourseId == course.ID).Select(e => e.StudentId).ToListAsync();
                var students = await _dataContext.Students.Where(s => studentIds.Contains(s.ID) && s.IsDeleted == false).ToListAsync();
                if (!students.Any())
                {
                    _logger.LogError("there are no students for this course");
                    throw new NullReferenceException("there are no students for this course");
                }
                return _mapper.Map<List<StudentResponseDto>>(students);
            }
            if (!String.IsNullOrWhiteSpace(request.InstructorName))
            {
                var instructor = await _dataContext.Instructors.Where(i => i.Name == request.InstructorName).FirstOrDefaultAsync();
                if (instructor == null)
                {
                    _logger.LogError("there are no instructors found");
                    throw new NullReferenceException("there are no instructors found");
                }
                var course = await _dataContext.Courses.Where(c => c.InstructorId == instructor.ID).FirstOrDefaultAsync();
                if (course == null)
                {
                    _logger.LogError("there are no courses for this instructor");
                    throw new NullReferenceException("there are no courses for this instructor");
                }
                var studentIds = await _dataContext.Enrollments.Where(e => e.CourseId == course.ID).Select(e => e.StudentId).ToListAsync();
                var students = await _dataContext.Students.Where(s => studentIds.Contains(s.ID) && s.IsDeleted == false).ToListAsync();
                if (!students.Any())
                {
                    _logger.LogError("there are no students for this course");
                    throw new NullReferenceException("there are no students for this course");
                }
                return _mapper.Map<List<StudentResponseDto>>(students);
            }

            throw new InvalidOperationException("No matching students found.");
        }

        public async Task<StudentResponseDto> GetStudent(StudentRequestDto request)
        {
            if (request.Email != null)
            {
                var student = await _dataContext.Students.Where(u => u.Email == request.Email && u.IsDeleted == false).SingleOrDefaultAsync();
                if (student != null) return _mapper.Map<StudentResponseDto>(student);
            }
            else if (request.Name != null)
            {
                var student = await _dataContext.Students.Where(u => u.Name == request.Name && u.IsDeleted == false).SingleOrDefaultAsync();
                if (student != null) return _mapper.Map<StudentResponseDto>(student);
            }

            throw new InvalidOperationException("an error occured while trying to retrieve the student");
        }

        public async Task<List<StudentResponseDto>> GetStudents()
        {
            var students = await _dataContext.Students.Where(s => s.IsDeleted == false).ToListAsync();
            if (students.Count == 0)
            {
                _logger.LogError("no student's were found");
                throw new ArgumentNullException("no student's were found");
            }
            var response = _mapper.Map<List<StudentResponseDto>>(students);

            return response;
        }

        public async Task<string> RemoveStudent(int id)
        {
            var student = await _dataContext.Students.SingleOrDefaultAsync(u => u.ID == id);
            if (student == null)
            {
                _logger.LogError($"Unable to remove student: {id}");
                throw new ArgumentNullException("no student has been found");
            }
            student.IsDeleted = true;
            await _dataContext.SaveChangesAsync();
            return "student has been removed successfully";
        }

        public async Task<StudentResponseDto> UpdateStudent(int id, StudentRequestDto request)
        {
            if (id == 0)
            {
                _logger.LogError("the ID is not valid");
                throw new ArgumentNullException("enter a valid ID");
            }
            var student = await _dataContext.Students.SingleOrDefaultAsync(u => u.ID == id);
            if (student == null)
            {
                _logger.LogError("no students found for this ID");
                throw new ArgumentNullException("no students found for this ID");
            }
            student.Name = String.IsNullOrWhiteSpace(request.Name)?student.Name:request.Name;
            student.Email = String.IsNullOrWhiteSpace(request.Email)?student.Email:request.Email;
            _dataContext.Students.Update(student);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<StudentResponseDto>(student);
        }
    }
}
