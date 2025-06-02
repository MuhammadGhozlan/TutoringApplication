using TutoringWebsite.API.DTOs;

namespace TutoringWebsite.API.Interfaces.IRepositories
{
    public interface ICourseRepository
    {
        Task<CourseResponseDto> CreateCourse(CourseRequestDto request);
        Task<bool> DeleteCourse(int id);
        Task<CourseResponseDto> UpdateCourse(int id, CourseRequestDto request);
        Task<CourseResponseDto> GetCourse(int id);
        Task<List<CourseResponseDto>> GetCourses();
        Task<List<CourseResponseDto>> GetFilteredCourse(CourseFilterRequestDto request);
    }
}
