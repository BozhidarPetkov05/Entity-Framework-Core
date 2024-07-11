using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogFluentAPI
{
    public class Blog
    {
        public int BlogId { get; set; }
        
        // null! is used to ignore the null exception
        public string Name { get; set; } = null!;
        
        //"?" used to say that the property is nullable (can contain null value) - only when nullable is enabled
        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
