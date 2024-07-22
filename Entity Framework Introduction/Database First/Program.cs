using Database_First.Infrastructure.Data;
using Database_First.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Database_First
{
    internal class Program
    {
        //Command
        //Scaffold-DbContext "Data Source=.\SQLEXPRESS;Database=SoftUni;User Id=DESKTOP-58IIM9O\\XidZ01;Password=;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -DataAnnotations -Context SoftUniDbContext -ContextDir Data -OutputDir Data/Models
        static async Task Main(string[] args)
        {
            //Getting all the tables from the SoftUni database
            using SoftUniDbContext context = new SoftUniDbContext();

            //Creating new employee
            //Employee emp = new Employee()
            //{
            //    FirstName = "Petar",
            //    LastName = "Petrov",
            //    DepartmentId = 1,
            //    HireDate = DateTime.Now,
            //    JobTitle = "Test",
            //    Salary = 12000
            //};
            ////Adding employee in the table Employees
            //await context.Employees.AddAsync(emp);
            
            //Getting employee where LastName is "Kulov"
            var emp1 = await context.Employees
                .Where(e => e.LastName == "Kulov")
                .FirstOrDefaultAsync();

            //Set the salary of the employee with LastName = "Kulov" to 0
            emp1.Salary = 0;

            //Save the changes
            await context.SaveChangesAsync();

            //Getting employee where LastName is "Petrov"
            var emp2 = await context.Employees
                .Where(e => e.LastName == "Petrov")
                .FirstOrDefaultAsync();

            //Removing employee from the table Employees
            context.Employees.Remove(emp2);

            //Save the changes
            await context.SaveChangesAsync();

            //Get the FirstName, LastName, JobTitle, Salary from all Employees
            var employees = await context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToListAsync();

            //Print the info of all Employees
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} ({employee.JobTitle}) - {employee.Salary}");
            }
        }
    }
}