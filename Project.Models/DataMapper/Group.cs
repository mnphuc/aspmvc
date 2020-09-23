using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool Status { get; set; }

        public  ICollection<Users> Users { get; set; }
        public  ICollection<GroupRoles> GroupRoles { get; set; }
    }
}
