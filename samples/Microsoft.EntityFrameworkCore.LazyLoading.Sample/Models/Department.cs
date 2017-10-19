using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Sample.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        private LazyReference<Instructor> _administratorLazy = new LazyReference<Instructor>();

        public Instructor Administrator
        {
            get
            {
                return _administratorLazy.GetValue(this);
            }
            set
            {
                _administratorLazy.SetValue(value);
            }
        }
        public ICollection<Course> Courses { get; set; }
    }
}
