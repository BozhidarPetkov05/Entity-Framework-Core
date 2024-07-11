using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataAnnotation
{
    [Table("Blogs", Schema = "blg")] //Setting the table name to "Blogs", setting the schema name to "blg"
    [Index(nameof(Name), IsUnique = true, Name = "IX_Blogs_Name_Unique")]
    public class Blog
    {
        [Key] //Setting PrimaryKey to BlogId Property
        public int BlogId { get; set; }

        [Required] //This property cannot accept null
        [MaxLength(50)] //Setting the max length to 50
        [Column("BlogName", TypeName = "NVARCHAR")] //Setting the column name to "BlogName", setting the type to "NVARCHAR"
        public string Name { get; set; } = null!; // null! is used to ignore the null exception

        [MaxLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string? Description { get; set; } //"?" used to say that the property is nullable (can contain null value) - only when nullable is enabled

        //For ValueGeneratedOnAdd we use FluentAPI
        public DateTime Created { get; set; }

        //For ValueGeneratedOnUpdate we use FluentAPI
        public DateTime LastUpdated { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; } = null!;

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
