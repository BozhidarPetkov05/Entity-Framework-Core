using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataAnnotation
{
    [Table("PostsTags", Schema = "blg")]
    public class PostTag
    {
        public int PostId { get; set; }
        
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;
        public int TagId { get; set; }
        
        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; } = null!;
    }
}
