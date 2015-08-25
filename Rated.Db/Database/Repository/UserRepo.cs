using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rated.Core.Models.User;
using Rated.Infrastructure.Database.EF.User;
using Rated.Core.Contracts;
using System.Data.Entity;
using Rated.Core.Shared;
using System.Data.Entity.Validation;

namespace Rated.Infrastructure.Database.Repository
{
    public class UserRepo : IUserRepo
    {
        UserContext _userContext;

        public UserRepo()
        {
            _userContext = new UserContext();
        }

        public UserCoreModel GetUserByUserId(Guid userId)
        {
            var usersDb = (from u in _userContext.Users
                           where u.UserId == userId
                           select u).SingleOrDefault();

            return new UserCoreModel()
            {
                FirstName = usersDb.FirstName,
                LastName = usersDb.LastName,
                Email = usersDb.Email,
                UserId = usersDb.UserId,
                Bio = usersDb.Bio,
            };
        }

        public List<UserCoreModel> GetUsers()
        { 
            var usersDb = (from u in _userContext.Users
                               select u).ToList();

            // Map to DB to core object
            var users = new List<UserCoreModel>();
            foreach (var user in usersDb)
            {
                users.Add(new UserCoreModel() { 
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserId = user.UserId,
                });
            }

            return users;
        }

        public List<UserCoreModel> SearchUsers(UserSearchCoreModel userSearch)
        {
            var usersDb = (from u in _userContext.Users

                           // First Name
                           where u.FirstName.Contains(
                               (String.IsNullOrEmpty(userSearch.FirstName) 
                               ? u.FirstName : userSearch.FirstName))

                           // Last Name
                           && u.LastName.Contains(
                           (String.IsNullOrEmpty(userSearch.LastName)
                           ? u.LastName : userSearch.LastName))

                           select u).ToList();

            // Map to DB to core object
            var users = new List<UserCoreModel>();
            foreach (var user in usersDb)
            {
                users.Add(new UserCoreModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserId = user.UserId,
                });
            }

            return users;
        }

        public UserCoreModel Login(string email, string password)
        {
            using (var userContext = _userContext)
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
                    Password = user.Password,
                    Bio = user.Bio,
                    LastLoginDate = user.LastLoginDate,
                    StatusId = (int)user.StatusId,
                };

                _userContext.Users.Add(userDb);
                _userContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errors = new StringBuilder();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errors.Append(String.Format("{0} - Error:{1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new Exception(errors.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EditAccount(UserCoreModel user)
        {
            using (var userContext = _userContext)
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
