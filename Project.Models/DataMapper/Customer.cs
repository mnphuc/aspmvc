using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required (ErrorMessage = "Bạn Chưa Nhập Email")]
        [RegularExpression(@"^(([\w])+(@)+([\w])+(.)+([a-zA-Z]{2,4}))$", ErrorMessage = "Email Sai Định Dạng!")]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn Chưa Nhập Mật Khẩu")]
        [Display(Name = "Mật Khẩu")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Mật Khẩu phải Lớn Hơn 6 ký tự")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bạn Chưa Nhập Họ Và Tên")]
        [Display(Name = "Họ và Tên")]
        public string FullName { get; set; }
        [Required (ErrorMessage = "Bạn Chưa Nhập Số Điện Thoại")]
        [RegularExpression(@"^(([07]||[08]||[09]||[03]||[05])+([\d]{8}))$", ErrorMessage = "Số Điện Thoại Không Đúng!")]
        [Display(Name = "Số Điện Thoại")]
        public string Phone { get; set; }
        [Required (ErrorMessage = "Bạn Chưa Nhập Địa Chỉ")]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }
        [Required (ErrorMessage = "Bạn Chưa Nhập GIới Tính")]
        [Display(Name = "Giới Tính")]
        public bool Gender { get; set; }
        [Display(Name = "Ngày Sinh")]
        public DateTime Birthday { get; set; }
        [Display(Name = "Ảnh Đại Diện")]
        public string Avartar { get; set; }
        public string Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? Update_at { get; set; }
        
        public bool? Status { get; set; }
        // thuộc tính liên kết
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
