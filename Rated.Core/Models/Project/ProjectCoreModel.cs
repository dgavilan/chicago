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
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public decimal Score { get; set; }
        public List<ProjectDetailCoreModel> ProjectDetails { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
