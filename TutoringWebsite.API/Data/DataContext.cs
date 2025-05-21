using Microsoft.EntityFrameworkCore;
using TutoringWebsite.API.Models;

namespace TutoringWebsite.API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().
                         HasOne<Instructor>().
                         WithMany().
                         HasForeignKey(c=>c.InstructorId).
                         OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Enrollment>().
                         HasOne<Student>().
                         WithMany().
                         HasForeignKey(e=>e.StudentId);
            modelBuilder.Entity<Enrollment>().
                         HasOne<Course>().
                         WithMany().
                         HasForeignKey(e => e.CourseId).
                         OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Payment>().
                         HasOne<Student>().
                         WithMany().
                         HasForeignKey(p => p.StudentId).
                         OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Payment>().
                         HasOne<Course>().
                         WithMany().
                         HasForeignKey(p => p.CourseId).
                         OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Payment>().
                         HasOne<Instructor>().
                         WithMany().
                         HasForeignKey(p => p.InstructorId).
                         OnDelete(DeleteBehavior.Restrict);
        }
    }
}
