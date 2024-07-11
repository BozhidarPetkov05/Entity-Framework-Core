using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDemo
{
    public class SoftUniDbContext : DbContext
    {
        public SoftUniDbContext()
        {
            
        }

        public SoftUniDbContext(DbContextOptions<SoftUniDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                string connectionString = "Server=DESKTOP-58IIM9O\\SQLEXPRESS;Database=SoftUni;User Id=DESKTOP-58IIM9O\\XidZ01;Password=;Trusted_Connection=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
