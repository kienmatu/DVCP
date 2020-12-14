using DVCP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DVCP.Repository
{
    public class InfoRepository
    {
        private DVCPContext entity;// = new DVCPContext();
        public InfoRepository(DVCPContext context)
        {
            this.entity = context;
        }
        
        public WebInfo FindByID(int id = 1)
        {
            WebInfo u = entity.WebInfo.Find(id);
            return u;
        }
       
        public void Update(WebInfo u)
        {
            entity.Entry(u).State = EntityState.Modified;
        }
    }
}