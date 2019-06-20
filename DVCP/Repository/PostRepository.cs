using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DVCP.Repository
{
    public class PostRepository
    {
        private DVCPContext entity;
        public PostRepository(DVCPContext context)
        {
            this.entity = context;
        }
        public void AddPost(Tbl_POST post)
        {
            entity.Tbl_POST.Add(post);
        }
        public IQueryable<Tbl_POST> AllPosts()
        {
            IQueryable<Tbl_POST> query = entity.Tbl_POST;
            return query.AsQueryable();
        }
        
        public void DeletePost(Tbl_POST post)
        {
            //entity.Tbl_Tags.Remove(post.Tbl_Tags);
            entity.Tbl_POST.Remove(post);
           
        }
        public void UpdatePost(Tbl_POST post)
        {
            entity.Entry(post).State = EntityState.Modified;
        }
        public Tbl_POST FindByID(int id)
        {
            return entity.Tbl_POST.Find(id);
        }
        public Tbl_POST FindBySlug(string slug)
        {
            return entity.Tbl_POST.Where(m => m.post_slug == slug).FirstOrDefault();
        }
        public void SaveChanges()
        {
            entity.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entity.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}