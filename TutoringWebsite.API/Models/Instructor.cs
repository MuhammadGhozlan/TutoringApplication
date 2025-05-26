using System.ComponentModel.DataAnnotations;

namespace TutoringWebsite.API.Models
{
    public class Instructor
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public required string Name { get; set; }
        public DateTime DoB { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        public required string UserId { get; set; }
        public required bool IsDeleted { get; set; } = false;
    }
}
