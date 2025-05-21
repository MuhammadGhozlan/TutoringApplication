using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IServices;

namespace TutoringWebsite.API.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IRegistrationService registrationService,
                                 ILogger<AccountController> logger)
        {
            _registrationService = registrationService;
            _logger = logger;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResultDto>> Registration(UserRegistrationDto registeredUser)
        {
            try
            {
                if(registeredUser == null)
                {
                    _logger.LogError("necessary information is not provided");
                    return BadRequest("provide the necessary information");
                }
                var registration = await _registrationService.Registration(registeredUser);
                return registration;
            }
            catch(Exception ex)
            {

                return BadRequest(new { ex.Message });
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResultDto>> Login(LoginRequestDto loginUser)
        {
            try
            {
                if(loginUser == null)
                {
                    _logger.LogError("necessary information is not provided");
                    return BadRequest("provide the necessary information");
                }
                var logUser = await _registrationService.Login(loginUser);
                return logUser;
            }
            catch(Exception ex)
            {

                return BadRequest(new { ex.Message });
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<ServiceResultDto>> Logout()
        {            
            return await _registrationService.Logout();
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("accessDenied")]
        public ActionResult AccessDenied()
        {
            return Forbid("access is denied for this user");
        }
    }
}
