namespace DVCP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_User()
        {
            Tbl_POST = new HashSet<Tbl_POST>();
        }

        [Key]
        public int userid { get; set; }

        [Required]
        [StringLength(20)]
        public string username { get; set; }

        [Required]
        [StringLength(100)]
        public string password { get; set; }

        [Required]
        [StringLength(30)]
        public string fullname { get; set; }

        [StringLength(20)]
        public string userrole { get; set; }

        public bool status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_POST> Tbl_POST { get; set; }
    }
}
