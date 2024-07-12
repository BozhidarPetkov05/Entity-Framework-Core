using Database_First.Infrastructure.Data;
using Database_First.Infrastructure.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

    }
}