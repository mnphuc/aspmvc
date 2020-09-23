using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Provider
    {
        [Key]
        public int ProviderId { get; set; }
        [Required]
        public string ProviderName { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        // thuộc tính liên kết
        public virtual ICollection<Product> Products { get; set; }
    }
}
