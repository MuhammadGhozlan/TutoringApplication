using System.ComponentModel.DataAnnotations;

namespace TutoringWebsite.API.DTOs
{
    public class LoginRequestDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string password { get; set; }        
    }
}
