using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Rated2.Models.Project
{
    public class ProjectViewModel
    {
        [DisplayName("Project Name")]
        public string Name { get; set; }

        [DisplayName("Project Description")]
        public string Description { get; set; }

        [DisplayName("Reviewer's Email")]
        public string ReviewerEmail { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ModifiedBy { get; set; }

        public List<ProjectItemViewModel> ProjectItems { get; set; }
    }
}