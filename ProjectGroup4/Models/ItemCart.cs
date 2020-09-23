using Project.Models.DataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectGroup4.Models
{
    public class ItemCart
    {
        public Product Product { get; set; }
        public string Attributes { get; set; }
        public int ColorId { get; set; }
        public int SizesId { get; set; }
        public int Quantity { get; set; }
    }
}