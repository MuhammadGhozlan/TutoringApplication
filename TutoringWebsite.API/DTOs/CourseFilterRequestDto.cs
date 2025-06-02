namespace TutoringWebsite.API.DTOs
{
    public class CourseFilterRequestDto
    {
        public string Title { get; set; }
        public int InstructorId { get; set; }
        public int NumberOfStudents { get; set; }
    }
}
