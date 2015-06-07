using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rated2.Models
{
    public class ProjectViewModel
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Score { get; set; }
    }
}