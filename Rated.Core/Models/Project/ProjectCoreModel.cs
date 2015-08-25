using Rated.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Models.Project
{
    public class ProjectCoreModel
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public decimal ProjectRating { get; set; }
        public int ProjectDetailsCount { get; set; }
        public Enums.ProjectStatus ProjectStatus { get; set; }
        public Enums.ReviewerProjectStatus ReviewerProjectStatus { get; set; }
        public List<TaskCoreModel> ProjectDetails { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public CompanyCoreModel Company { get; set; }

        // Company info
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }

        public void MoveToNextStatus()
        {
            switch (this.ProjectStatus)
            {
                case Enums.ProjectStatus.New:
                    this.ProjectStatus = Enums.ProjectStatus.Draft;
                    break;
                case Enums.ProjectStatus.Draft:
                    this.ProjectStatus = Enums.ProjectStatus.InProgress;
                    break;
                case Enums.ProjectStatus.InProgress:
                    this.ProjectStatus = Enums.ProjectStatus.Done;
                    break;
            }
        }

    }
}
