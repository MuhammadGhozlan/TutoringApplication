using System.ComponentModel.DataAnnotations;

namespace TutoringWebsite.API.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }
        [Required, MaxLength(100)]
        public required string Name { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string UserId { get; set; }
        public required bool IsDeleted { get; set; } = false;
    }
}
