using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P02_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.Property(u => u.Username)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(u => u.Password)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(25);

            builder.Property(u => u.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(80);

            builder.Property(u => u.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(u => u.Balance)
                .IsRequired();
        }
    }
}
