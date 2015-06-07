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
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Score { get; set; }
        public List<ProjectDetailCoreModel> ProjectDetails { get; set; }
    }
}
