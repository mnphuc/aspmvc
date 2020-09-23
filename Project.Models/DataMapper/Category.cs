using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
      
        public string MetaLink { get; set; }
        [Required]
        public int DisplayNo { get; set; }
        public string FeatureImg { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
   
        [Required]
        public bool ShowHome { get; set; }
        // thuộc tính liên kết
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
