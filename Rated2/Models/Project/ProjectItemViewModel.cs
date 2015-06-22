using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Rated2.Models.Project
{
    public class ProjectItemViewModel
    {
        public string ProjectDetailName { get; set; }
        public string ProjectDetailDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ModifiedBy { get; set; }
    }
}