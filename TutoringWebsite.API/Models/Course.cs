using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TutoringWebsite.API.Models
{
    public class Course
    {
        [Key]
        public int ID { get; set; }
        [Required, MaxLength(200)]
        public required string Title { get; set; }        
        public int InstructorId { get; set; }
    }
}
