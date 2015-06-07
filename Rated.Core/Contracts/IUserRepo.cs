using Rated.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Contracts
{
    public interface IUserRepo
    {
        UserCoreModel Login(string email, string password);

        void CreateUser(UserCoreModel user);
    }
}
