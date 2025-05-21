namespace TutoringWebsite.API.DTOs
{
    public class UserRegistrationDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required AppRole Role { get; set; }
    }
    public enum AppRole
    {
        Admin,
        Student,
        Instructor
    }
}
