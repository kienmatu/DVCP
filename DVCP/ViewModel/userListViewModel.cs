using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DVCP.ViewModel
{
    public class userListViewModel
    {
        public int userid { get; set; }

        [Required(ErrorMessage = "Chưa nhập username, max 20 ký tự")]
        [StringLength(20)]
        public string username { get; set; }

        [Required(ErrorMessage = "Chưa nhập password")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Compare("password",ErrorMessage = "Bạn nhập mật khẩu không trùng nhau!")]
        public string repassword { get; set; }

        [Required(ErrorMessage = "Chưa nhập fullname")]
        [StringLength(30)]
        public string fullname { get; set; }

        [StringLength(20)]
        public string userrole { get; set; }
    }
}