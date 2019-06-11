using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVCP
{
    public class GetData
    {
        UnitOfWork db = new UnitOfWork(new DVCPContext());
        public List<Models.Tbl_POST> GetPopularPost()
        {
            return db.postRepository.AllPosts().Take(5).Where(m => m.status == true).OrderByDescending(m => m.ViewCount).ToList();
        }
        public List<Models.Tbl_POST> GetPostByTag(int tag, int count)
        {
            Tbl_Tags tags = db.tagRepository.FindByID(tag);
            return db.postRepository.AllPosts().Take(count > 3 ? count : 3).Where(m => m.Tbl_Tags.Contains(tags) && m.status == true).ToList();
        }
        public Tbl_HotPost[] GetHotPosts()
        {
            Tbl_HotPost[] post = db.hotPostRepository.AllPosts().Where(m => m.Tbl_POST.status == true).OrderBy(m => m.priority).Take(3).ToArray();
            return post;
        }
        public IEnumerable<ViewModel.ListTagViewModel> GetListTags()
        {
            return db.tagRepository.AllTags().Select(
                m => new ViewModel.ListTagViewModel
                {
                    TagID = m.TagID,
                    TagName = m.TagName
                }).ToList();
        }
        public IEnumerable<EnumShowList> GetEnumShowList()
        {
            List<ViewModel.Dynasty> dynasties = GetEnumList<ViewModel.Dynasty>();
            List<EnumShowList> enums = new List<EnumShowList>();
            foreach(var i in dynasties)
            {
                enums.Add(new EnumShowList
                {
                    slug = SlugGenerator.SlugGenerator.GenerateSlug(i.GetDisplayName()),
                    showname = i.GetDisplayName(),
                    value = (int)i,

                });
            }
            return enums;
        }
        private static List<T> GetEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new List<T>(array);
            return list;
        }
    }
    public class EnumShowList
    {
        public string slug { get; set; }
        public string showname { get; set; }
        public int value { get; set; }

    }
}