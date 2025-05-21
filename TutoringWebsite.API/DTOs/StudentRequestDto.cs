using System.ComponentModel.DataAnnotations;

namespace TutoringWebsite.API.DTOs
{
    public class StudentRequestDto
    {
        public required string Name { get; set; }
        
        public required string Email { get; set; }
        
        public string? UserId { get; set; }
    }
}
