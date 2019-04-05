using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVCP.ViewModel
{
    public enum PostType : int
    {
        [Display(Name = "Bài Viết Bình Thường")]
        Normal = 1, // Bình thường
        [Display(Name ="Slide Hình Ảnh")]
        Silde = 2, // Slide ảnh
        [Display(Name = "Tranh Cãi, Thảo Luận")]
        Discuss = 3, // Tranh cãi, thảo luận
    }
    public enum Rated : int
    {
        [Display(Name = "Bài Viết Bình Thường")]
        Normal = 3, 
        [Display(Name = "Đề Xuất Cao")]
        HighRated = 2,
        [Display(Name = "Đề Xuất Quan Trọng")]
        HighestRated = 1,
    }
    public enum Dynasty
    {
        [Display(Name = "Thời Hồng Bàng")]
        HongBang,
        [Display(Name = "Nhà Đinh")]
        Dinh,
        [Display(Name = "Nhà Tiền Lê")]
        Early_Le,
        [Display(Name = "Nhà Lý")]
        Ly,
        [Display(Name = "Nhà Trần")]
        Tran,
        [Display(Name = "Nhà Hậu Lê")]
        Later_Le,
        [Display(Name = "Nhà Lê Trung Hưng")]
        LeTrungHung,
        [Display(Name = "Nhà Tây Sơn")]
        TaySon,
        [Display(Name = "Nhà Nguyễn")]
        Nguyen,
    }
    
    
    public class newPostViewModel

    {
      
        
        public int post_id { get; set; }

        public int? userid { get; set; }

        [Required]
        [StringLength(200)]
        public string post_title { get; set; }

        [Required]
        [StringLength(500)]
        public string post_teaser { get; set; }

        [StringLength(200)]
        public string AvatarImage { get; set; }
        [StringLength(500)]
        public string post_review { get; set; }

        [AllowHtml]
        public string post_content { get; set; }

        [Range(1, 3, ErrorMessage = "Vui lòng chọn đúng kiểu bài viết!")]
        [Required(ErrorMessage ="Vui lòng chọn kiểu bài viết!")]
        public PostType post_type { get; set; }

        
        public List<SelectListItem> post_tag { get; set; }

        public DateTime? create_date { get; set; }

        public DateTime? edit_date { get; set; }

       
        public Dynasty dynasty { get; set; }
        public Rated Rated { get; set; }
    }
}