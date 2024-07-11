using MiniORM.App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniORM.App
{
    public class SoftUniDbContext : DbContext
    {
        public SoftUniDbContext(string connectionString) 
            : base(connectionString)
        {
            
        }
        public DbSet<Department> Department { get; } = null!;
        public DbSet<Employee> Employees { get; } = null!;  
    }
}
