using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DVCP.Repository
{
    public class UserRepository
    {
        private DVCPContext entity;// = new DVCPContext();
        public UserRepository(DVCPContext context)
        {
            this.entity = context;
        }
        public void Add(tbl_User user)
        {
            entity.tbl_User.Add(user);
        }
        public tbl_User FindByUsername(string user)
        {
            tbl_User u = entity.tbl_User.Where(x => x.username == user).FirstOrDefault();
            return u;
        }
        public tbl_User FindByID(int id)
        {
            tbl_User u = entity.tbl_User.Find(id);
            return u;
        }
        public IQueryable<tbl_User> AllUsers()
        {
            IQueryable<tbl_User> query = entity.tbl_User;
            return query.AsQueryable();
        }
        public void Delete(tbl_User user)
        {
            entity.tbl_User.Remove(user);
        }
        public void Update(tbl_User u)
        {
            entity.Entry(u).State = EntityState.Modified;
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