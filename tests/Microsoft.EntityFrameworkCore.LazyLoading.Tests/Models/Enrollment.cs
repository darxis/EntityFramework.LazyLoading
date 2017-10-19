using System.ComponentModel.DataAnnotations;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        private readonly LazyReference<Course> _courseLazy = new LazyReference<Course>();
        public Course Course
        {
            get => _courseLazy.GetValue(this);
            set => _courseLazy.SetValue(value);
        }
        private readonly LazyReference<Student> _studentLazy = new LazyReference<Student>();
        public Student Student
        {
            get => _studentLazy.GetValue(this);
            set => _studentLazy.SetValue(value);
        }
    }
}
