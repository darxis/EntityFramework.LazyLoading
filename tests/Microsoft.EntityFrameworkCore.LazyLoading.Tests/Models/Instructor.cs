using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests.Models
{
    public class Instructor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public ICollection<CourseAssignment> CourseAssignments { get; set; }
        private readonly LazyReference<OfficeAssignment> _officeAssignmentLazy = new LazyReference<OfficeAssignment>();
        public OfficeAssignment OfficeAssignment
        {
            get => _officeAssignmentLazy.GetValue(this, nameof(OfficeAssignment));
            set => _officeAssignmentLazy.SetValue(value);
        }
    }
}
