using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class AttributeType
    {
        [Key]
        public int AtttypeId { get; set; }
        [Required]
        public string TypeName { get; set; }
        public int OrderBy { get; set; }
        public bool Status { get; set; }
        [Required]
        public bool? Important { get; set; }
        public virtual ICollection<Attributes> Attributes { get; set; }
    }
}
