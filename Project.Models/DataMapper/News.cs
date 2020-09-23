using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
   public class News
    {
        [Key]
        public int PostId { get; set; }
        [Required]
        public string PostTitle { get; set; }
        [Required]
        public string PostContent { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string FeatureImg { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool Status { get; set; }
    }
}
