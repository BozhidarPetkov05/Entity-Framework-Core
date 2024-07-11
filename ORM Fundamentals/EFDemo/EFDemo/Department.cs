using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDemo
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public int ManagerID { get; set; }
        [ForeignKey(nameof(ManagerID))]
        public Employee Manager { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
