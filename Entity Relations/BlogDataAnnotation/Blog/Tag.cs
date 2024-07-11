using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataAnnotation
{
    [Table("Tags", Schema = "blg")]
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        
        [Required]
        [MaxLength(15)]
        public string TagName { get; set; } = null!;

        public List<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
