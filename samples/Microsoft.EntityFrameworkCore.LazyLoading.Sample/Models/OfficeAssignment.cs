using System.ComponentModel.DataAnnotations;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Sample.Models
{
    public class OfficeAssignment
    {
        [Key]
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        private LazyReference<Instructor> _instructorLazy = new LazyReference<Instructor>();
        public Instructor Instructor
        {
            get
            {
                return _instructorLazy.GetValue(this);
            }
            set
            {
                _instructorLazy.SetValue(value);
            }
        }
    }
}
