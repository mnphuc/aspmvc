using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        [Required]
        public string SizeName { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateAt { get; set; }
        // thuộc tính liên kết
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
