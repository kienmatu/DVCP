using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVCP.CommonData
{
    public class PostData
    {
        public static List<SelectListItem> getTagList()
        {
            UnitOfWork db = new UnitOfWork(new DVCPContext());
            List<SelectListItem> lstTag = db.tagRepository.AllTags()
                .Select(m =>
                new SelectListItem
                {
                    Text = m.TagName,
                    Value = m.TagID.ToString(),
                }
                ).ToList();
            return lstTag;
            //    return new List<SelectListItem>
            //{
            //    new SelectListItem {Text = "Kiến trúc", Value = "Kiến trúc"},
            //    new SelectListItem {Text = "Chất liệu", Value = "Chất liệu"},
            //    new SelectListItem {Text = "Trang phục", Value = "Trang phục"},
            //    new SelectListItem {Text = "Binh bị", Value = "Binh bị"},
            //    new SelectListItem {Text = "Quân sự", Value = "Quân sự"},
            //    new SelectListItem {Text = "Thần thoại", Value = "Thần thoại"},
            //    new SelectListItem {Text = "Văn hóa", Value = "Văn hóa"},
            //    new SelectListItem {Text = "Phong tục", Value = "Phong tục"},
            //    new SelectListItem {Text = "Tôn giáo", Value = "Tôn giáo"},

            //};
            //List<SelectListItem> lstDynasty = new List<SelectListItem>
            //{
            //    //"Kiến trúc","Chất liệu","Trang phục","Binh bị","Quân sự","Thần thoại","Văn hóa","Dã sử","Phong tục"
            //};
        }
    }
}