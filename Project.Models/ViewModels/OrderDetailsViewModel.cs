using Project.Models.DataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Money { get; set; }
    }
}
