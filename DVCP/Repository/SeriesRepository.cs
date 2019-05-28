using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DVCP.Repository
{
    public class SeriesRepository
    {
        private DVCPContext entity;
        public SeriesRepository(DVCPContext context)
        {
            this.entity = context;
        }
        public void AddSeries(Tbl_Series series)
        {
            entity.Tbl_Series.Add(series);
        }
        public IQueryable<Tbl_Series> AllSeries()
        {
            IQueryable<Tbl_Series> query = entity.Tbl_Series;
            return query.AsQueryable();
        }
        public void Delete(Tbl_Series post)
        {
            entity.Tbl_Series.Remove(post);
        }
        public void Update(Tbl_Series post)
        {
            entity.Entry(post).State = EntityState.Modified;
        }
        public Tbl_Series FindByID(int id)
        {
            return entity.Tbl_Series.Find(id);
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