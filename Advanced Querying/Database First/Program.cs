using Database_First.Infrastructure.Data;
using Database_First.Infrastructure.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Database_First
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using SoftUniDbContext context = new SoftUniDbContext();

            //Demo with Int
            DemoQueryWithInt(context);

            //Demo with String
            DemoQueryWithString(context);

            //Demo with Interpolated String
            DemoWithInterpolatedString(context);

            //Creating Stored Procedures
            CreatingStoredProcedure();

            //Executing Stored Procedures
            ExecutingStoredProcedures(context);

            //ReatachingObjects
            ReattachingObjects();

            //Loading Objects Explicitly - Loading the objects when we need them
            ExplicitLoading();

            //Eager Loading - Loading all the date at once
            EagerLoading();

            //Lazy Loading - Delays loading of data until it's used, we don't know when or how many queries it makes
            LazyLoading();
        }

        static void DemoQueryWithInt(SoftUniDbContext context)
        {
            int departmentId = 1;
            string query = "SELECT * FROM Employees WHERE DepartmentId = " + departmentId;
            var employees = context.Employees
                .FromSqlRaw(query)
                .ToList();

            Console.WriteLine($"Demo with int count: {employees.Count}");
        }

        static void DemoQueryWithString(SoftUniDbContext context)
        {
            string jobTitle = "Marketing Assistant";
            string query = "SELECT * FROM Employees WHERE JobTitle = {0}";
            var employees = context.Employees
                .FromSqlRaw(query, jobTitle)
                .ToList();
            
            Console.WriteLine($"Demo with normal string count: {employees.Count}");
        }

        static void DemoWithInterpolatedString(SoftUniDbContext context)
        {
            string jobTitle = "Marketing Assistant";
            FormattableString query = $"SELECT * FROM Employees WHERE JobTitle = {jobTitle}";
            var employees = context.Employees
                .FromSqlInterpolated(query)
                .ToList();
            
            Console.WriteLine($"Demo with interpolated string count: {employees.Count}");
        }
        static void CreatingStoredProcedure()
        {
            try
            {
                using (SoftUniDbContext context = new SoftUniDbContext())
                {
                    string query = "CREATE PROC TestProcedure @p INT AS SELECT TOP(@p) * FROM Employees";
                    context.Database.ExecuteSqlRaw(query);
                }
                Console.WriteLine("Created Stored Procedure");
            }
            catch (Exception)
            {
                Console.WriteLine("There is already a procedure with this name!");
            }
            
        }

        static void ExecutingStoredProcedures(SoftUniDbContext context)
        {
            SqlParameter percent = new SqlParameter("@percent", 10);
            string procQuery = "EXEC UpdateSalary @percent";
            int rowsAffected = context.Database.ExecuteSqlRaw(procQuery, percent);

            Console.WriteLine($"Rows Affected From Stored Procedure: {rowsAffected}");
        }

        static void ReattachingObjects()
        {
            Employee? employee;
            using (SoftUniDbContext context = new SoftUniDbContext())
            {
                employee = context.Find<Employee>(1);
            }

            if (employee != null)
            {
                employee.MiddleName = "Ralf";
                
                using SoftUniDbContext ctx = new SoftUniDbContext();

                var entry = ctx.Entry(employee);
                entry.State = EntityState.Modified;

                ctx.SaveChanges();

                Console.WriteLine("Reattaching Object: Saved Changes");
            }
        }

        static void ExplicitLoading()
        {
            Employee? employee;
            using (SoftUniDbContext context = new SoftUniDbContext())
            {
                //Getting the employee data only
                employee = context.Employees.Find(1);
                var entry = context.Entry(employee);

                //Loading the address of the current employee
                entry.Reference(e => e.Address).Load();
                Console.WriteLine("Address Loaded.");

                //Loading all the projects of the employee
                entry.Collection(e => e.Projects).Load();
                Console.WriteLine("Projects Loaded.");
            }
        }

        static void EagerLoading()
        {
            using (SoftUniDbContext context = new SoftUniDbContext())
            {
                //Loading the department with eager loading
                var employees = context.Employees
                    .Where(e => e.DepartmentId == 1)
                    .Include(e => e.Department) //We want to use the Department property
                    .ToList();
                
                Console.WriteLine(" ");
                Console.WriteLine("Eager Loading:");

                //Loading the address with explicit loading
                foreach (var employee in employees)
                {
                    if (employee.EmployeeId == 3)
                    {
                        var entry = context.Entry(employee);
                        entry.Reference(e => e.Address).Load();

                        Console.WriteLine($"{employee.FirstName} {employee.LastName}, {employee.Department.Name}, {employee.Address.AddressText}");
                    }
                    else
                    {
                        Console.WriteLine($"{employee.FirstName} {employee.LastName}, {employee.Department.Name}");
                    }
                }
            }
        }

        static void LazyLoading()
        {
            using (SoftUniDbContext context = new SoftUniDbContext())
            {
                var employees = context.Employees
                    .Where(e => e.DepartmentId == 1)
                    .ToList();

                Console.WriteLine(" ");
                Console.WriteLine("Lazy Loading:");

                foreach (var employee in employees)
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.Department.Name}");
                }
            }
        }

        static void ConcurrencyCheck()
        {
            using SoftUniDbContext context1 = new SoftUniDbContext();
            using SoftUniDbContext context2 = new SoftUniDbContext();

            var pr1 = context1.Employees
                .Where(e => e.EmployeeId == 1)
                .Select(e => e.Projects.First())
                .FirstOrDefault();

            pr1.Name = "First";

            var pr2 = context2.Employees
                .Where(e => e.EmployeeId == 1)
                .Select(e => e.Projects.First())
                .FirstOrDefault();

            pr2.Name = "Second";

            context1.SaveChanges();
            context2.SaveChanges();
        }
    }
}