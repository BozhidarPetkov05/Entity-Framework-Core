using Database_First.Infrastructure.Data;
using Database_First.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Database_First
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using SoftUniDbContext context = new SoftUniDbContext();

            //Classic LINQ
            /*
            var employees = from employee in context.Employees
                            where employee.DepartmentId == 1
                            select employee;
            */

            //LINQ With Extension Methods
            /*
            var employees = context.Employees
                .Where(e => e.DepartmentId == 1)
                .ToList();
            */

            //Query With Less Requested Data - Anonymous Object
            /*
            var employees = context.Employees
                .Where(e => e.DepartmentId == 1)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList();
            */

            //Query With Less Requested Data - Data Transfer Object
            /*
            var employees = context.Employees
                .Where(e => e.DepartmentId == 1)
                .Select(e => new EmployeeDTO()
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary,
                })
                .ToList();
            */

            //Check Count Of All Employees With DepartmentId = 1 - With Where
            /*
            var count = context.Employees
                .Where(e => e.DepartmentId == 1)
                .Count();
            */

            //Check Count Of All Employees With DepartmentId = 1 - Without Where
            var count = context.Employees
                .Count(e => e.DepartmentId == 1);

            //Get Average Salary Of All Employees With DepartmentId = 1
            var avgSalary = context.Employees
                .Where(e => e.DepartmentId == 1)
                .Average(e => e.Salary);

            //Get Max Salary Of All Employees With DepartmentId = 1
            var maxSalary = context.Employees
                .Where(e => e.DepartmentId == 1)
                .Max(e => e.Salary);

            //Get Min Salary Of All Employees With DepartmentId = 1
            var minSalary = context.Employees
                .Where(e => e.DepartmentId == 1)
                .Min(e => e.Salary);

            //Get Sum Salary Of All Employees With DepartmentId = 1
            var sumSalary = context.Employees
                .Where(e => e.DepartmentId == 1)
                .Sum(e => e.Salary);

            //Query Without Join()
            /*
            var employees = context.Employees
                .Where(e => e.DepartmentId == 1)
                .Select(e => new EmployeeDTO()
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary,
                })
                .ToList();
            */

            //Query With Join()
            var employees = context.Employees
                .Where(e => e.DepartmentId == 1)
                .Join(
                    context.Departments,
                    e => e.DepartmentId,
                    d => d.DepartmentId,
                    (e, d) => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        e.Salary,
                        e.DepartmentId
                    })
                .ToList();

            Console.WriteLine($"Total: {count}");
            Console.WriteLine($"Average: {avgSalary}");
            Console.WriteLine($"Max: {maxSalary}");
            Console.WriteLine($"Min: {minSalary}");
            Console.WriteLine($"Sum: {sumSalary}");
            Console.WriteLine(" ");

            Console.WriteLine("Employees:");
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} ({employee.JobTitle}) - {employee.Salary}");
            }
        }
    }
}