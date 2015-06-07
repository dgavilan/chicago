using Rated2.Models.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rated2.Models
{
    public class UserProfileViewModel
    {
        public ProfileViewModel Profile { get; set; }
        public List<ProjectViewModel> Projects { get; set; }
    }
}