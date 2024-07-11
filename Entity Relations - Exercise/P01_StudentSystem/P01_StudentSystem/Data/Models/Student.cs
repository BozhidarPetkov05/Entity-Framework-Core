using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("Name", TypeName = "NVARCHAR")]
        public string Name { get; set; } = null!;

        [MaxLength(10)]
        [MinLength(10)]
        [Column("PhoneNumber", TypeName = "VARCHAR")]
        public string? PhoneNumber { get; set; }

        [Required] 
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; }
    }
}
