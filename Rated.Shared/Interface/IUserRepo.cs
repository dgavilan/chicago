using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Shared.Interface
{
    public interface IUserRepo
    {
        bool Login(string email, string password);
        //void CreateUser(UserCoreModel user);
    }
}
