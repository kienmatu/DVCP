using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVCP.ViewModel
{
    public class infoViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Nhập tên website")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string web_name { get; set; }

        [MaxLength(200,ErrorMessage = "Tối đa 200 ký tự")]
        [Required(ErrorMessage = "Nhập mô tả website")]
        public string web_des { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Nhập mô tả website chi tiết")]
        public string web_about { get; set; }
    }
}