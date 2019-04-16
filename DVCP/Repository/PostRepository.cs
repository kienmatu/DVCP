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
        public void AddPost(tbl_POST post)
        {
            entity.tbl_POST.Add(post);
        }
        public IQueryable<tbl_POST> AllPosts()
        {
            IQueryable<tbl_POST> query = entity.tbl_POST;
            return query.AsQueryable();
        }
        public void DeletePost(tbl_POST post)
        {
            entity.tbl_POST.Remove(post);
        }
        public void UpdatePost(tbl_POST post)
        {
            entity.Entry(post).State = EntityState.Modified;
        }
        public tbl_POST FindByID(int id)
        {
            return entity.tbl_POST.Find(id);
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