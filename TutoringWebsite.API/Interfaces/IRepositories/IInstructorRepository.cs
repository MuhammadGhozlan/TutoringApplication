using TutoringWebsite.API.DTOs;

namespace TutoringWebsite.API.Interfaces.IRepositories
{
    public interface IInstructorRepository
    {
        Task<InstructorResponseDto> AddInstructor(InstructorRequestDto instructorRequestDto);
        Task<string> RemoveInstructor(int id);
        Task<InstructorResponseDto> UpdateInstructor(int id, InstructorRequestDto instructorRequestDto);
        Task<InstructorResponseDto> GetInstructor(InstructorRequestDto instructorRequestDto);
        Task<List<InstructorResponseDto>> GetInstructors();
        Task<List<InstructorResponseDto>> GetFilteredInstructors(InstructorFilterRequestDto instructorFilterRequestDto);
    }
}
