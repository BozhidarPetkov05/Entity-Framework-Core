using BlogFluentAPI.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogFluentAPI
{
    public class BlogDbContext : DbContext
    {
        //Constructor - empty 
        public BlogDbContext()
        {
            
        }

        //Constructor used to set the options
        public BlogDbContext(DbContextOptions<BlogDbContext> options) 
            : base(options)
        {
            
        }

        //Method for creating the model of the database
        //FluentAPI is used here
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Applying configuration to the database
            modelBuilder.ApplyConfiguration(new BlogConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        //Mehtod used to configure the options for the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                string connectionString = "Server=DESKTOP-58IIM9O\\SQLEXPRESS;Database=BlogDb;User Id=DESKTOP-58IIM9O\\XidZ01;Integrated Security=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}
