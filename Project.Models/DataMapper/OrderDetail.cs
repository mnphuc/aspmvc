using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        // thuộc tính liên kết
        [ForeignKey("OrderId")]
        public virtual Orders Orders { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }



        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }
        [ForeignKey("ColorId")]
        public virtual Color Color { get; set; }

    }
}
