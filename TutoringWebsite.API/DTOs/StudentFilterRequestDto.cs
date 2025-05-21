using System.ComponentModel.DataAnnotations;

namespace TutoringWebsite.API.DTOs
{
    public class StudentFilterRequestDto
    {        
        public int CourseId { get; set; }
        
        public string? CourseName { get; set; }
        
        public int InstructorId { get; set; }
        public string? InstructorName { get; set; }
    }
}
