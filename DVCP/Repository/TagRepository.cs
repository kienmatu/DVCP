using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DVCP.Repository
{
    public class TagRepository
    {
        private DVCPContext entity;
        public TagRepository(DVCPContext context)
        {
            this.entity = context;
        }
        public void AddTag(Tbl_Tags tag)
        {
            entity.Tbl_Tags.Add(tag);
        }
        public IQueryable<Tbl_Tags> AllTags()
        {
            IQueryable<Tbl_Tags> query = entity.Tbl_Tags;
            return query.AsQueryable();
        }
        public void DeletePost(Tbl_Tags post)
        {
            entity.Tbl_Tags.Remove(post);
        }
        public void UpdateTag(Tbl_Tags tag)
        {
            entity.Entry(tag).State = EntityState.Modified;
        }
        public Tbl_Tags FindByID(int id)
        {
            return entity.Tbl_Tags.Find(id);
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