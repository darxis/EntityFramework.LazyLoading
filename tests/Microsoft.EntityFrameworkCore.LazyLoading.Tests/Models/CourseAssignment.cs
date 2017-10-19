namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Models
{
    public class CourseAssignment
    {
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        private readonly LazyReference<Instructor> _instructorLazy = new LazyReference<Instructor>();
        public Instructor Instructor
        {
            get => _instructorLazy.GetValue(this);
            set => _instructorLazy.SetValue(value);
        }
        private readonly LazyReference<Course> _courseLazy = new LazyReference<Course>();
        public Course Course
        {
            get => _courseLazy.GetValue(this);
            set => _courseLazy.SetValue(value);
        }
    }
}
