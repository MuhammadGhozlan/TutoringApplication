using TutoringWebsite.API.DTOs;

namespace TutoringWebsite.API.Interfaces.IRepositories
{
    public interface IStudentService
    {
        Task<StudentResponseDto> AddStudent(StudentRequestDto request);
        Task<string> RemoveStudent(int id);
        Task<StudentResponseDto> UpdateStudent(int id, StudentRequestDto request);
        Task<StudentResponseDto> GetStudent(StudentRequestDto request);
        Task<List<StudentResponseDto>> GetStudents();
        Task<List<StudentResponseDto>> FilterStudents(StudentFilterRequestDto request);
    }
}
