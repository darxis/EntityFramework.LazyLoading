using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int CourseId { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public int DepartmentId { get; set; }

        private readonly LazyReference<Department> _departmentLazy = new LazyReference<Department>();
        public Department Department
        {
            get => _departmentLazy.GetValue(this, nameof(Department));
            set => _departmentLazy.SetValue(value);
        }

        public ICollection<Enrollment> Enrollments { get; set; }

        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
