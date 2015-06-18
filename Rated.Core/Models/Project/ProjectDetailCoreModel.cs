using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Models.Project
{
    public class ProjectDetailCoreModel
    {
        public Guid ProjectDetailId { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectDetailName { get; set; }
        public string ProjectDetailDescription { get; set; }
        public string TimeToComplete { get; set; }
        public string Details { get; set; }
        public decimal Score { get; set; }

        public Guid UserId { get; set; }

    }
}
