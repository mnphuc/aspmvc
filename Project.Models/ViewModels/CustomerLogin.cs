using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.ViewModels
{
    public class CustomerLogin
    {
            [Required (ErrorMessage = "Bạn Chưa Nhập Email")]
            public string Email { get; set; }
            [Required (ErrorMessage = "Bạn Chưa Nhập Mật Khẩu")]
            public string Password { get; set; }   
    }
}
