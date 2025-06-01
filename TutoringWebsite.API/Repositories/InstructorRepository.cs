using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TutoringWebsite.API.Data;
using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IRepositories;
using TutoringWebsite.API.Models;

namespace TutoringWebsite.API.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<InstructorRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IdentityContext _identityContext;

        public InstructorRepository(DataContext dataContext,
                                    ILogger<InstructorRepository> logger,
                                    IMapper mapper,
                                    IdentityContext identityContext)
        {
            _dataContext = dataContext;
            _logger = logger;
            _mapper = mapper;
            _identityContext = identityContext;
        }
        public async Task<InstructorResponseDto> AddInstructor(InstructorRequestDto request)
        {
            if(request == null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email) || request.DoB == null)
            {
                _logger.LogError("request body or an essential part of it is null");
                throw new ArgumentNullException("check the request body and fill the essential parts");
            }
            var userId = await _identityContext.Users.Where(u => u.Email == request.Email).Select(u => u.Id).SingleOrDefaultAsync();
            if(userId == null)
            {
                _logger.LogError("you need to register the user first");
                throw new InvalidOperationException("check if the user is registered");
            }
            var instructor = new Instructor
            {
                Name = request.Name,
                Email = request.Email,
                DoB = request.DoB.Value,
                UserId = userId,
                IsDeleted = false
            };

            await _dataContext.Instructors.AddAsync(instructor);
            await _dataContext.SaveChangesAsync();
            var response = _mapper.Map<InstructorResponseDto>(instructor);
            return response;
        }

        public async Task<List<InstructorResponseDto>> GetFilteredInstructors(InstructorFilterRequestDto request)
        {
            if(request == null)
            {
                _logger.LogError("you need to choose a filter");
                throw new ArgumentNullException("one filter at least should not be null");
            }
            if(request.CourseId != 0)
            {
                var instructorIds = await _dataContext.Courses.Where(c => c.ID == request.CourseId).Select(c => c.InstructorId).ToListAsync();
                var instructors = await _dataContext.Instructors.Where(i => instructorIds.Contains(i.ID) && i.IsDeleted == false).ToListAsync();
                return _mapper.Map<List<InstructorResponseDto>>(instructors);
            }
            if(!String.IsNullOrWhiteSpace(request.CourseName))
            {
                var instructorIds = await _dataContext.Courses.Where(c => c.Title == request.CourseName).Select(c => c.InstructorId).ToListAsync();
                var instructors = await _dataContext.Instructors.Where(i => instructorIds.Contains(i.ID) && i.IsDeleted == false).ToListAsync();
                return _mapper.Map<List<InstructorResponseDto>>(instructors);
            }
            if(request.NumberOfStudents > 0)
            {
                var coursesStudents = _dataContext.Enrollments.GroupBy(e => e.CourseId).Select(s => new { CourseId = s.Key, StudentCount = s.Count() });
                var coursesIds = await coursesStudents.Where(cs => cs.StudentCount == request.NumberOfStudents).Select(cs => cs.CourseId).ToListAsync();
                var instructorsIds = await _dataContext.Courses.Where(c => coursesIds.Contains(c.ID)).Select(c => c.InstructorId).ToListAsync();
                var instructors = await _dataContext.Instructors.Where(i => instructorsIds.Contains(i.ID) && i.IsDeleted == false).ToListAsync();
                return _mapper.Map<List<InstructorResponseDto>>(instructors);
            }

            throw new NotImplementedException("an error occured while implementing the request");
        }

        public async Task<InstructorResponseDto> GetInstructor(InstructorRequestDto request)
        {
            if(request == null)
            {
                _logger.LogError("the request body is null");
                throw new ArgumentNullException("the request body is null");
            }
            if(!String.IsNullOrWhiteSpace(request.Name))
            {
                var instructor = await _dataContext.Instructors.SingleOrDefaultAsync(i => i.Name == request.Name && i.IsDeleted == false);
                return _mapper.Map<InstructorResponseDto>(instructor);
            }
            if(!String.IsNullOrWhiteSpace(request.Email))
            {
                var instructor = await _dataContext.Instructors.SingleOrDefaultAsync(i => i.Email == request.Email && i.IsDeleted == false);
                return _mapper.Map<InstructorResponseDto>(instructor);
            }

            throw new NotImplementedException("error occured during the execution of the request");
        }

        public async Task<List<InstructorResponseDto>> GetInstructors()
        {
            var instructors = await _dataContext.Instructors.Where(i => i.IsDeleted == false).ToListAsync();
            return _mapper.Map<List<InstructorResponseDto>>(instructors);
        }

        public async Task<string> RemoveInstructor(int id)
        {
            if(id == 0)
            {
                _logger.LogError("invalid Id");
                throw new InvalidOperationException("invalid Id, enter a valid Id and try again");
            }
            var instructor = await _dataContext.Instructors.SingleOrDefaultAsync(i => i.ID == id && i.IsDeleted == false);
            if(instructor == null)
            {
                _logger.LogError("no instructor was found");
                throw new InvalidOperationException("no instructor was found");
            }
            instructor.IsDeleted = true;
            _dataContext.Instructors.Update(instructor);
            _dataContext.SaveChanges();
            return "the instructor was removed";
        }

        public async Task<InstructorResponseDto> UpdateInstructor(int id, InstructorRequestDto request)
        {
            if(id == 0)
            {
                _logger.LogError("invalid Id");
                throw new InvalidOperationException("invalid Id, enter a valid Id and try again");
            }
            if(request == null)
            {
                _logger.LogError("the request body is null");
                throw new ArgumentNullException("the request body is null");
            }
            var instructor = await _dataContext.Instructors.SingleOrDefaultAsync(i => i.ID == id && i.IsDeleted == false);
            if(instructor == null)
            {
                _logger.LogError("no instructor was found");
                throw new InvalidOperationException("no instructor was found");
            }
            if(!String.IsNullOrWhiteSpace(request.Name))
            {
                instructor.Name = request.Name;
            }
            if(!String.IsNullOrWhiteSpace(request.Email))
            {
                instructor.Email = request.Email;
            }
            if(request.DoB != null)
            {
                instructor.DoB = request.DoB.Value;
            }
            _dataContext.Instructors.Update(instructor);
            _dataContext.SaveChanges();
            return _mapper.Map<InstructorResponseDto>(instructor);
        }
    }
}
