using Rated.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Models.User
{
    public class UserCoreService
    {
        IUserRepo _userRepo;

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public Guid UserId { get; private set; }
        public bool LoggedIn { get; private set; }

        public UserCoreService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public bool IsLoggedIn()
        {
            return LoggedIn;
        }

        public UserCoreModel Login(string email, string userPassword)
        {
            var user = _userRepo.Login(email, userPassword);

            if (user.UserId != Guid.Empty)
            {
                return new UserCoreModel()
                {
                    CreatedDate = user.CreatedDate,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    IsActive = user.IsActive,
                    LastName = user.LastName,
                    ModifiedDate = user.ModifiedDate,
                    UserId = user.UserId,
                    LastLoginDate = user.LastLoginDate,
                    Bio = user.Bio
                };
            }
            else
            {
                return new UserCoreModel();
            }
        }

        public void ClearSession()
        {
            UserId = Guid.NewGuid();
            FirstName = "";
            LastName = "";
            Email = "";
            LoggedIn = false;
        }

        public void EditAccount(UserCoreModel user)
        {
            _userRepo.EditAccount(user);
        }
    }
}
