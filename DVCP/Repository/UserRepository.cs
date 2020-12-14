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
        public void Add(User user)
        {
            entity.Users.Add(user);
        }
        public User FindByUsername(string user)
        {
            User u = entity.Users.Where(x => x.username == user).FirstOrDefault();
            return u;
        }
        public User FindByID(int id)
        {
            User u = entity.Users.Find(id);
            return u;
        }
        public IQueryable<User> AllUsers()
        {
            IQueryable<User> query = entity.Users;
            return query.AsQueryable();
        }
        public void Delete(User user)
        {
            entity.Users.Remove(user);
        }
        public void Update(User u)
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