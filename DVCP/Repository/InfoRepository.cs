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
        
        public info FindByID(int id = 1)
        {
            info u = entity.info.Find(id);
            return u;
        }
       
        public void Update(info u)
        {
            entity.Entry(u).State = EntityState.Modified;
        }
    }
}