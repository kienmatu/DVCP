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
            return db.postRepository.AllPosts().Take(5).OrderByDescending(m => m.ViewCount).ToList();
        }
        public List<Models.Tbl_POST> GetPostByTag(int tag,int count)
        {
            Tbl_Tags tags = db.tagRepository.FindByID(tag);
            return db.postRepository.AllPosts().Take(count > 3 ? count : 3 ).Where(m => m.Tbl_Tags.Contains(tags)).ToList();
        }
    }
}