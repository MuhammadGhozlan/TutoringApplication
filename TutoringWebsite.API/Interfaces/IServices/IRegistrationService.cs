using TutoringWebsite.API.DTOs;

namespace TutoringWebsite.API.Interfaces.IServices
{
    public interface IRegistrationService
    {
        Task<ServiceResultDto> Registration(UserRegistrationDto request);
        Task<ServiceResultDto> Login(LoginRequestDto request);
        Task<ServiceResultDto> Logout();
    }
}
