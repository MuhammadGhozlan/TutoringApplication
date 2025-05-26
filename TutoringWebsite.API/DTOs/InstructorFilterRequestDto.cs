namespace TutoringWebsite.API.DTOs
{
    public class InstructorFilterRequestDto
    {
        public int CourseId { get; set; }
        
        public string? CourseName { get; set; }

        public int NumberOfStudents { get; set; }
    }
}
