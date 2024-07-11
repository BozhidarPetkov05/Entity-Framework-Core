﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BlogFluentAPI.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            //builder = modelBuilder.Entity<Blog>()

            builder
            .HasKey(b => b.BlogId);

            builder
                .ToTable("Blogs", "blg");

            builder
                .Property(b => b.Name)
                .HasColumnName("BlogName")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50)
            .IsRequired();

            builder
                .Property(b => b.Description)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(500);

            builder
                .Property(b => b.Created)
                .ValueGeneratedOnAdd();

            builder           
                .Property(b => b.LastUpdated)
                .ValueGeneratedOnUpdate();
        }
    }
}
