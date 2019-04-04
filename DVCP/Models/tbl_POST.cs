namespace DVCP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_POST
    {
        [Key]
        public int post_id { get; set; }

        public int? userid { get; set; }

        [Required]
        [StringLength(200)]
        public string post_title { get; set; }

        [Required]
        [StringLength(500)]
        public string post_teaser { get; set; }

        [StringLength(500)]
        public string post_review { get; set; }

        [Column(TypeName = "ntext")]
        public string post_content { get; set; }

        public int post_type { get; set; }

        [StringLength(200)]
        public string post_tag { get; set; }

        public DateTime? create_date { get; set; }

        public DateTime? edit_date { get; set; }

        [StringLength(50)]
        public string dynasty { get; set; }

        public virtual tbl_User tbl_User { get; set; }
        public decimal ViewCount { get; set; }
    }
}
