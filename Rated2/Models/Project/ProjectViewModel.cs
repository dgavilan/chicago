using Rated.Core.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Rated2.Models.Project
{
    public class ProjectViewModel
    {
        public Guid ProjectId { get; set; }

        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        [DisplayName("Project Description")]
        public string ProjectDescription { get; set; }

        [DisplayName("Reviewer's Email")]
        public string ReviewerEmail { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public int ProjectDetailsCount { get; set; }
        public Enums.ProjectStatus ProjectStatus { get; set; }

        public List<ProjectDetailViewModel> ProjectDetails { get; set; }
    }
}