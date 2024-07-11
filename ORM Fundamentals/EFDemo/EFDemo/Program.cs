using Microsoft.EntityFrameworkCore.Storage;
using System.Data.SqlClient;

namespace EFDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new SoftUniDbContext();

            var employee = context.Employees.Find(1);
            employee.IsDeleted = false;
            context.SaveChanges();
            
            
            /*var employees = context.Employees
                .Where(e => e.DepartmentId == 3)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    DepartmentName = e.Department.Name
                })
                .ToList();

            foreach (var employee in employees )
            {
                Console.WriteLine($"{employee.DepartmentName} - {employee.FirstName} {employee.LastName}: {employee.JobTitle}");
            }*/
        }
    }
}