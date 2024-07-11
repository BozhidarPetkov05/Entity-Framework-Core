using MiniORM.App.Entities;

namespace MiniORM.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=DESKTOP-58IIM9O\\SQLEXPRESS;Database=SoftUni;User ID=DESKTOP-58IIM9O\\XidZ01;Password=;TrustServerCertificate=True;"
            var dbContext = new SoftUniDbContext(connectionString);
            
            var departments = new Department[]
            {
                new Department { Id = 1, Name = "First"},
                new Department { Id = 2, Name = "Second"}
            };
            var changeTracker = new ChangeTracker<Department>(departments);

            foreach (var (original, copy) in departments.Zip(changeTracker.AllEntities))
            {
                original.Id = -1;
                Console.WriteLine(ReferenceEquals(original, copy));
            }

        }
    }
}