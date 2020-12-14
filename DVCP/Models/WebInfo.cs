namespace DVCP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("info")]
    public partial class WebInfo
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string web_name { get; set; }

        [StringLength(200)]
        public string web_des { get; set; }

        [Column(TypeName = "ntext")]
        public string web_about { get; set; }
    }
}
