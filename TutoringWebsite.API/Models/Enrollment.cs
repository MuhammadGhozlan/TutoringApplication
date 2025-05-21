using System.ComponentModel.DataAnnotations;

namespace TutoringWebsite.API.Models
{
    public class Enrollment
    {
        [Key]
        public int ID { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
    }
}
