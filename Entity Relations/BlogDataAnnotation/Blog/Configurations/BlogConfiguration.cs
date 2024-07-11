using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataAnnotation.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        { 
            builder
                .Property(b => b.Created)
                .ValueGeneratedOnAdd();

            builder           
                .Property(b => b.LastUpdated)
                .ValueGeneratedOnUpdate();
        }
    }
}
