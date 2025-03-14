using Domain.Interfaces;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _databaseContext;
        public UserRepository(DatabaseContext databaseContext) 
        {
            _databaseContext = databaseContext;
        }

        public User GetUserById(int id)
        {
            return _databaseContext.Users.Where(u => u.Id == id).FirstOrDefault();
        }
    }
}
