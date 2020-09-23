using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.DataMapper
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [RegularExpression(@"^([\w]+@[\w]+.[a-zA-Z]{2,4})$", ErrorMessage = "Emmail Không đúng định dạng!")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 8)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }
        [Display(Name = "Họ Và Tên")]
        public string FullName { get; set; }
        [Display(Name = "Quyền")]
        public int GroupId { get; set; }
        [Required]
        [RegularExpression(@"^(([07]||[08]||[09]||[03]||[05])+([\d]{8}))$", ErrorMessage = "Số Điện Thoại Không Đúng!")]
        [Display(Name = "Số Điện Thoại")]
        public string Phone { get; set; }
        [Display(Name = "Ảnh Đại Diện")]
        public string Avatar { get; set; }
        [Display(Name = "Ngày Sinh")]
        public DateTime Birthday { get; set; }
        public DateTime? CreateDate { get; set; }
        [Display(Name = "Giới Tính")]
        public bool Gender { get; set; }
        //public int GroupId { get; set; }
        [Display(Name = "Trạng Thái")]
        public int Status { get; set; }
        // thuộc tính liên kết
        [ForeignKey("GroupId")]
        public Group Groups { get; set; }
        //[ForeignKey("GroupId")]
        //public Group Groups { get; set; }

    }
}
