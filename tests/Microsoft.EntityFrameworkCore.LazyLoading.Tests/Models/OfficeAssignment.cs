using System.ComponentModel.DataAnnotations;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Models
{
    public class OfficeAssignment
    {
        [Key]
        public int InstructorId { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        private readonly LazyReference<Instructor> _instructorLazy = new LazyReference<Instructor>();
        public Instructor Instructor
        {
            get => _instructorLazy.GetValue(this, nameof(Instructor));
            set => _instructorLazy.SetValue(value);
        }
    }
}
