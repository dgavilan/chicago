using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rated.Models.Project
{
    public class ReviewerViewModel
    {
        public Guid ReviewerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public decimal Rating { get; set; }
        public string Comments { get; set; }
    }
}