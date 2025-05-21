using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutoringWebsite.API.Data;
using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IServices;
using TutoringWebsite.API.Models;

namespace TutoringWebsite.API.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegistrationService> _logger;
        private readonly DataContext _dataContext;


        public RegistrationService(UserManager<User> userManager,
                                   SignInManager<User> signInManager,
                                   RoleManager<IdentityRole> roleManager,
                                   ILogger<RegistrationService> logger,
                                   DataContext dataContext
                                   )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _dataContext = dataContext;
        }

        public async Task<ServiceResultDto> Registration(UserRegistrationDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user != null)
            {
                _logger.LogError("user already exists");
                ServiceResultDto serviceResultDto = new ServiceResultDto
                {
                    Success = false,
                    Message = "user already exists"

                };
                return serviceResultDto;
            }
            var newUser = new User
            {
                UserName = request.Username,
                Email = request.Email,
            };
            await _userManager.CreateAsync(newUser, request.Password);
            var roleExist = await _roleManager.RoleExistsAsync(request.Role.ToString());
            if(!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(request.Role.ToString()));
            }
            await _userManager.AddToRoleAsync(newUser, request.Role.ToString());
            await _signInManager.SignInAsync(newUser, isPersistent: false);
            ServiceResultDto result = new ServiceResultDto
            {
                Success = true,
                Message = "user created successfully"
            };
            return result;

        }

        public async Task<ServiceResultDto> Login(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                _logger.LogError("user does not exist. you need to register first");
                throw new Exception("user does not exist");
            }
            var student = await _dataContext.Students.SingleOrDefaultAsync(u => u.Email == request.Email);
            if(student != null && student.IsDeleted == true)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;
                await _userManager.UpdateAsync(user);
                return new ServiceResultDto
                {
                    Success = false,
                    Message = "user has been deleted"
                };
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.password, true, false);
            if(!result.Succeeded)
            {
                _logger.LogWarning("Invalid Credentials");
                throw new Exception("Invalid Email or Password");
            }
            return new ServiceResultDto
            {
                Success = true,
                Message = "login successful"
            };
        }
        public async Task<ServiceResultDto> Logout()
        {
            await _signInManager.SignOutAsync();
            return new ServiceResultDto
            {
                Success = true,
                Message = "logout successful"
            };
        }
    }
}
