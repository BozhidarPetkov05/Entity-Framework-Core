using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataAnnotation
{
    [Table("Authors", Schema = "blg")]
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AuthorName { get; set; } = null!;
        
        public int? BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog? Blog { get; set; }
    }
}
