using System.ComponentModel.DataAnnotations;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Sample.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        private LazyReference<Course> _courseLazy = new LazyReference<Course>();
        public Course Course
        {
            get
            {
                return _courseLazy.GetValue(this);
            }
            set
            {
                _courseLazy.SetValue(value);
            }
        }
        private LazyReference<Student> _studentLazy = new LazyReference<Student>();
        public Student Student
        {
            get
            {
                return _studentLazy.GetValue(this);
            }
            set
            {
                _studentLazy.SetValue(value);
            }
        }
    }
}
