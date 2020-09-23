using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public float PriceIn { get; set; }
        public float PriceOut { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Discount { get; set; }
        [Required]
        public string Images { get; set; }
        public string muti_image { get; set; }
        public string Description { get; set; }   
        [Required]
        public int ProviderId { get; set; }
        public DateTime? CreateDate { get; set; }
      
        public string MetaLink { get; set; }
        public int CoutView { get; set; }
        public bool Status { get; set; }
        // thuộc tính liên kết
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }
        
        [ForeignKey("ProviderId")]
        public virtual Provider Provider { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
