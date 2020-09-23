using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class ProductAttribute
    {
        [Key, Column(Order = 1)]
        public int ProductId { get; set; }
        [Key, Column(Order = 2)]
        public int AttributeId { get; set; }
        // thuộc tính liên kết
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("AttributeId")]
        public virtual Attributes Attribute { get; set; }
    }
}
