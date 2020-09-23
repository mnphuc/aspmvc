using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [Required]
        public string ColorName { get; set; }
        [Required]
        public string background { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateAt { get; set; }
        // thuộc tính liên kết
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
