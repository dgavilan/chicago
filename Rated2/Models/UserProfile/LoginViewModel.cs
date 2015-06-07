using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Rated2.Models.UserProfile
{
    public class LoginViewModel
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        public string UserPassword { get; set; }
    }
}