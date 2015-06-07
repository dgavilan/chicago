using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rated2.Models.UserProfile
{
    public class ProfileViewModel
    {
        public Guid UserId { get; set; }
        public decimal Score { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Password")]
        public string UserPassword { get; set; }
    }
}