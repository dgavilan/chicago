using Rated.Core.Models.User;
using System;
using System.Web;

namespace Rated.Web.Shared
{
    public class UserSession
    {
        
        public bool IsLoggedIn()
        {
            if (HttpContext.Current.Session["UserSession"] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void InitializeUserSession(UserCoreModel user)
        {
            if (user.UserId != Guid.Empty)
            {
                HttpContext.Current.Session["UserSession"] = user;
            }
        }

        public void ClearUserSession()
        {
            HttpContext.Current.Session["UserSession"] = null;
        }

        public UserCoreModel GetUserSession()
        {
            return (UserCoreModel)HttpContext.Current.Session["UserSession"];
        }
    }
}