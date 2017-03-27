namespace Microsoft.EntityFrameworkCore.LazyLoading.Sample.Models
{
    public class CourseAssignment
    {
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        private LazyReference<Instructor> _instructorLazy = new LazyReference<Instructor>();
        public Instructor Instructor
        {
            get
            {
                return _instructorLazy.GetValue(this, nameof(Instructor));
            }
            set
            {
                _instructorLazy.SetValue(value);
            }
        }
        private LazyReference<Course> _courseLazy = new LazyReference<Course>();
        public Course Course
        {
            get
            {
                return _courseLazy.GetValue(this, nameof(Course));
            }
            set
            {
                _courseLazy.SetValue(value);
            }
        }
    }
}
