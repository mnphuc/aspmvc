using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
   public class GroupRoles
    {
        [Column(Order = 0), Key]
        public int GroupId { get; set; }
        [Column(Order = 1), Key]
        public string BusinessId { get; set; }
        [Column(Order = 2), Key]
        public string RoleId { get; set; }

        [ForeignKey("GroupId")]
        public Group Groups { get; set; }
        [ForeignKey("BusinessId")]
        public Business Businesses { get; set; }
        [ForeignKey("RoleId")]
        public Role Roles { get; set; }
    }
}
