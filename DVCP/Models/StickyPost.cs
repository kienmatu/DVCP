namespace DVCP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StickyPost
    {
        public int id { get; set; }

        public int priority { get; set; }

        public int? post_id { get; set; }

        public virtual Post Tbl_POST { get; set; }
    }
}
