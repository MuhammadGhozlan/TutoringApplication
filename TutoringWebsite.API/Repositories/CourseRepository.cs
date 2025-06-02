using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutoringWebsite.API.Data;
using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IRepositories;
using TutoringWebsite.API.Models;
using TutoringWebsite.API.Services;

namespace TutoringWebsite.API.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext _dataContext;
        private readonly IdentityDbContext _identityDbContext;
        private readonly ILogger<CourseRepository> _logger;
        private readonly IMapper _mapper;
        public CourseRepository(DataContext dataContext,
                                IdentityDbContext identityDbContext,
                                ILogger<CourseRepository> logger,
                                IMapper mapper)
        {
            _dataContext = dataContext;
            _identityDbContext = identityDbContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<CourseResponseDto> CreateCourse(CourseRequestDto request)
        {
            if(request.Title == null || request.InstructorId == 0)
            {
                _logger.LogError("request or a part of it is null");
                throw new ArgumentNullException("request or a part of it is null");
            }
            var instructor = await _dataContext.Instructors.FindAsync(request.InstructorId);
            if(instructor == null)
            {
                _logger.LogError("no instructor can be found for this instructor Id");
                throw new InvalidOperationException("no instructor can be found for this instructor Id");
            }
            var course = new Course
            {
                Title = request.Title,
                InstructorId = request.InstructorId,
            };
            await _dataContext.Courses.AddAsync(course);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<CourseResponseDto>(course);
        }

        public async Task<bool> DeleteCourse(int id)
        {
            var course = await _dataContext.Courses.SingleOrDefaultAsync(c => c.ID == id);
            if(course == null)
            {
                _logger.LogError("no course was found");
                throw new InvalidOperationException("no course was found");
            }
            _dataContext.Courses.Remove(course);
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<CourseResponseDto> GetCourse(int id)
        {
            if(id == 0)
            {
                _logger.LogError("Id cannot be null");
                throw new ArgumentNullException("Id cannot be null");
            }

            var course = await _dataContext.Courses.SingleOrDefaultAsync(c => c.ID == id);
            if(course == null)
            {
                _logger.LogError("Course not found");
                throw new InvalidOperationException("Course not found");
            }
            return _mapper.Map<CourseResponseDto>(course);
        }

        public async Task<List<CourseResponseDto>> GetCourses()
        {
            var courses = await _dataContext.Courses.ToListAsync();

            if(courses.Count == 0)
                _logger.LogWarning("No courses found");

            return _mapper.Map<List<CourseResponseDto>>(courses);
        }

        public async Task<List<CourseResponseDto>> GetFilteredCourse(CourseFilterRequestDto request)
        {
            if(request == null)
            {
                _logger.LogError("request cannot be null");
                throw new ArgumentNullException("request cannot be null");
            }
            IQueryable<Course> query = _dataContext.Courses.AsQueryable();

            if(!String.IsNullOrWhiteSpace(request.Title))
                query = query.Where(c => c.Title.ToLower() == request.Title.ToLower());

            if(request.InstructorId != 0)
            {
                var instructor = await _dataContext.Instructors.AnyAsync(i => i.ID == request.InstructorId);
                if(!instructor)
                {
                    _logger.LogError("no instructor can be found for this instructor Id");
                    throw new InvalidOperationException("no instructor can be found for this instructor Id");
                }
                query = query.Where(c => c.InstructorId == request.InstructorId);
            }
            if(request.NumberOfStudents > 0)
            {


            }
            var courses = await query.ToListAsync();
            return _mapper.Map<List<CourseResponseDto>>(courses);
        }

        public async Task<CourseResponseDto> UpdateCourse(int id, CourseRequestDto request)
        {
            if(id == 0)
            {
                _logger.LogError("Id cannot be null");
                throw new ArgumentNullException("Id cannot be null");
            }

            var course = await _dataContext.Courses.SingleOrDefaultAsync(c => c.ID == id);
            if(course == null)
            {
                _logger.LogError("Course not found");
                throw new InvalidOperationException("Course not found");
            }
            var instructorExists = await _dataContext.Instructors.AnyAsync(i => i.ID == request.InstructorId);


            course.Title = !String.IsNullOrWhiteSpace(request.Title) ? request.Title : course.Title;
            course.InstructorId = request.InstructorId != 0 && instructorExists ? request.InstructorId : course.InstructorId;

            await _dataContext.SaveChangesAsync();
            return _mapper.Map<CourseResponseDto>(course);
        }
    }
}
