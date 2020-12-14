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
        public void AddPost(Post post)
        {
            entity.Posts.Add(post);
        }
        public IQueryable<Post> AllPosts()
        {
            IQueryable<Post> query = entity.Posts;
            return query.AsQueryable();
        }
        
        public void DeletePost(Post post)
        {
            //entity.Tbl_Tags.Remove(post.Tbl_Tags);
            entity.Posts.Remove(post);
           
        }
        public void UpdatePost(Post post)
        {
            entity.Entry(post).State = EntityState.Modified;
        }
        public Post FindByID(int id)
        {
            return entity.Posts.Find(id);
        }
        public Post FindBySlug(string slug)
        {
            return entity.Posts.Where(m => m.post_slug == slug).FirstOrDefault();
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