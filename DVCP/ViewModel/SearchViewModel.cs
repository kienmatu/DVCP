using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVCP.ViewModel
{
    public class SearchViewModel
    {
        public string title { get; set; }
        public Dynasty? Dynasty { get; set; }
        public string tags { get; set; }
    }
}