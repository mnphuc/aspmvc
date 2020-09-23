using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Slider
    {
        [Key]
        public int SliderId { get; set; }
        public string SliderName { get; set; }
        [Required]
        public string SliderImg { get; set; }
        public int DisplayNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool Status { get; set; }
    }
}
