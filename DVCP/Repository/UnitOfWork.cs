using DVCP.Models;
using DVCP.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVCP
{
    public  class UnitOfWork
    {
        private DVCPContext context = new DVCPContext();
        private UserRepository _userRepository;
        private PostRepository _postRepository;
        public UnitOfWork(DVCPContext _context)
        {
            this.context = _context;
        }
        public PostRepository postRepository
        {
            get
            {
                if(_postRepository== null)
                {
                    _postRepository = new PostRepository(context);
                    
                }
                return _postRepository;
            }
        }
        public UserRepository userRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(context);

                }
                return _userRepository;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}