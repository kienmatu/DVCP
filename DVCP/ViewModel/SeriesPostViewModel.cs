using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVCP.ViewModel
{
    public class SeriesPostViewModel
    {
        public string SerieName { get; set; }
        public int SerieID { get; set; }
        public List<SeriesPost> ListPost { get; set; }
    }
    public class SeriesPost
    {
        public int post_id { get; set; }
        public int? userid { get; set; }
        public string post_title { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? edit_date { get; set; }
        public int ViewCount { get; set; }
        public bool status { get; set; }
        public string userfullname { get; set; }
        public string username { get; set; }
        public string slug { get; set; }
    }
}