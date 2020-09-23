using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Business
    {
        [Key]
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public bool Status { get; set; }

        public ICollection<GroupRoles> GroupRoles { get; set; }
    }
}
