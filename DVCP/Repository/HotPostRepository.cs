using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DVCP.Repository
{
    public class HotPostRepository
    {
        private DVCPContext entity;
        public HotPostRepository(DVCPContext context)
        {
            this.entity = context;
        }
        public void AddHotPost(Tbl_HotPost post)
        {
            entity.Tbl_HotPost.Add(post);
        }
        public IQueryable<Tbl_HotPost> AllPosts()
        {
            IQueryable<Tbl_HotPost> query = entity.Tbl_HotPost;
            return query.AsQueryable();
        }
        public void DeletePost(Tbl_HotPost post)
        {
            entity.Tbl_HotPost.Remove(post);

        }
        public void UpdatePost(Tbl_HotPost post)
        {
            entity.Entry(post).State = EntityState.Modified;
        }
        public Tbl_HotPost FindByID(int id)
        {
            return entity.Tbl_HotPost.Find(id);
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