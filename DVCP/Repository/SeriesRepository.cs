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
        public void AddSeries(Series series)
        {
            entity.Series.Add(series);
        }
        public IQueryable<Series> AllSeries()
        {
            IQueryable<Series> query = entity.Series;
            return query.AsQueryable();
        }
        public void Delete(Series post)
        {
            entity.Series.Remove(post);
        }
        public void Update(Series post)
        {
            entity.Entry(post).State = EntityState.Modified;
        }
        public Series FindByID(int id)
        {
            return entity.Series.Find(id);
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