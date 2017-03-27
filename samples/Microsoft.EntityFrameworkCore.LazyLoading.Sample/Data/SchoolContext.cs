using Microsoft.EntityFrameworkCore.LazyLoading.Sample.Models;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Sample.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>();
            modelBuilder.Entity<Enrollment>();
            modelBuilder.Entity<Student>();
            modelBuilder.Entity<Department>();
            modelBuilder.Entity<Instructor>();
            modelBuilder.Entity<OfficeAssignment>();
            modelBuilder.Entity<CourseAssignment>();
            modelBuilder.Entity<Person>();

            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID });
        }
    }
}
