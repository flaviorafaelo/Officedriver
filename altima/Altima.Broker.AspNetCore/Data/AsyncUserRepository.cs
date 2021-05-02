using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Altima.Broker.System.Account;
using Altima.Broker.System.Type;

namespace Altima.Broker.AspNet.Mvc.Data
{
    public class AsyncUserRepository : AsyncRepository<User>
    {
        public AsyncUserRepository(DataContext dbContext): base(dbContext)
        {

        }

        public User Login(Username username, Password password)
        {
            var result = _dbSet
                 .Where(u => u.Username.Value == username.Value && u.Password.Value == password.Value)
                 .FirstOrDefault();

            return result;
        }
    }
}
