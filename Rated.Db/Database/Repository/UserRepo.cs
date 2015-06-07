using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rated.Core.Models.User;
using Rated.Infrastructure.Database.EF;
using Rated.Core.Contracts;
using System.Data.Entity;

namespace Rated.Infrastructure.Database.Repository
{
    public class UserRepo : IUserRepo
    {
        UserContext _userModelDbContext;

        public UserRepo()
        {
            _userModelDbContext = new UserContext();
        }

        public UserCoreModel Login(string email, string password)
        {
            using (var userContext = _userModelDbContext)
            {
                var userDb = (from u in userContext.Users
                              where u.Email == email
                              && u.Password == password
                              && u.IsActive == true
                              select u).SingleOrDefault();

                if (userDb != null)
                {
                    // NOTE: Update last login date
                    var userToUpdate = (from u in userContext.Users
                                  where u.Email == email
                                  select u).SingleOrDefault();

                    userToUpdate.LastLoginDate = DateTime.UtcNow;
                    userContext.Entry(userToUpdate).State = EntityState.Modified;
                    userContext.SaveChanges();

                    return new UserCoreModel()
                    {
                        CreatedDate = userDb.CreatedDate,
                        Email = userDb.Email,
                        FirstName = userDb.FirstName,
                        IsActive = userDb.IsActive,
                        LastName = userDb.LastName,
                        ModifiedDate = userDb.ModifiedDate,
                        UserId = userDb.UserId,
                        LastLoginDate = userDb.LastLoginDate,
                        Bio = userDb.Bio
                    };
                }
                else
                {
                    return new UserCoreModel();
                }
            } //using
        }

        public void CreateUser(UserCoreModel user)
        {
            try
            {

                var userDb = new User()
                {
                    CreatedDate = user.CreatedDate,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    IsActive = user.IsActive,
                    LastName = user.LastName,
                    ModifiedDate = user.ModifiedDate,
                    UserId = user.UserId,
                    Password = user.Password
                };

                _userModelDbContext.Users.Add(userDb);
                _userModelDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EditAccount(UserCoreModel user)
        {
            using (var userContext = _userModelDbContext)
            {
                var userToUpdate = (from u in userContext.Users
                                    where u.Email == user.Email
                                    select u).SingleOrDefault();

                userToUpdate.Email = user.Email;
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.Bio = user.Bio;
                userToUpdate.ModifiedDate = user.ModifiedDate;

                userContext.Entry(userToUpdate).State = EntityState.Modified;
                userContext.SaveChanges();
            }
        }


        // TODO:
        // 2. Encrypt password
        // 3. Modify profile
    }
}
