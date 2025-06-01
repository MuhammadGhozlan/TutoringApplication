using Azure.Core;
using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IRepositories;
using TutoringWebsite.API.Interfaces.IServices;

namespace TutoringWebsite.API.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly ILogger<InstructorService> _logger;
        private readonly IInstructorRepository _instructorRepo;

        public InstructorService(ILogger<InstructorService> logger,
                              IInstructorRepository instructorRepo)
        {
            _logger = logger;
            _instructorRepo = instructorRepo;
        }
        public async Task<InstructorResponseDto> AddInstructor(InstructorRequestDto request)
        {
            if(request == null || String.IsNullOrEmpty(request.Email) || String.IsNullOrEmpty(request.Name) || request.DoB == null)
            {
                _logger.LogError("request cannot be null");
                throw new InvalidOperationException("request is null");
            }

            var instructor = await _instructorRepo.AddInstructor(request);

            return instructor;
        }

        public async Task<List<InstructorResponseDto>> GetFilteredInstructors(InstructorFilterRequestDto request)
        {
            if(request == null)
            {
                _logger.LogError("you have to choose a filter");
                throw new InvalidOperationException("you have to choose a filter");
            }
            var instructors = await _instructorRepo.GetFilteredInstructors(request);
            if(instructors.Count == 0)
            {
                _logger.LogError("there are no instructors for this course");
                throw new InvalidOperationException("there are no instructors for this course");
            }
            return instructors;
        }

        public async Task<InstructorResponseDto> GetInstructor(InstructorRequestDto request)
        {
            if(request == null)
            {
                _logger.LogError("you have to choose a filter");
                throw new InvalidOperationException("you have to choose a filter");
            }

            var instructor = await _instructorRepo.GetInstructor(request);
            if(instructor == null)
            {
                _logger.LogError("no instructor was found");
                throw new InvalidOperationException("no instructor was found");
            }
            return instructor;
        }

        public async Task<List<InstructorResponseDto>> GetInstructors()
        {
            var instructors = await _instructorRepo.GetInstructors();
            if(instructors.Count == 0)
            {
                _logger.LogError("no instructors were found");
                throw new InvalidOperationException("no instructors were found");
            }
            return instructors;
        }

        public async Task<string> RemoveInstructor(int id)
        {
            if(id == 0)
            {
                _logger.LogError("you have to enter a valid Id");
                throw new InvalidOperationException("Id entered is not valid Id");
            }

            var instructor = await _instructorRepo.RemoveInstructor(id);

            return instructor;
        }

        public async Task<InstructorResponseDto> UpdateInstructor(int id, InstructorRequestDto request)
        {
            if(request == null || id == 0)
            {
                _logger.LogError("either the id is not valid or the entered request is incorrect");
                throw new InvalidOperationException("either the id is not valid or the entered request is incorrect");
            }

            var instructor = await _instructorRepo.UpdateInstructor(id, request);
           
            return instructor;
        }
    }
}
