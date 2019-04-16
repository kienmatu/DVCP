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

        [Required]
        [StringLength(20)]
        [Index(IsUnique = true)]
        public string username { get; set; }

        [Required]
        [StringLength(100)]
        public string password { get; set; }

        [Required]
        [StringLength(30)]
        public string fullname { get; set; }

        [StringLength(20)]
        public string userrole { get; set; }
        public int countPost { get; set; }
    }
}