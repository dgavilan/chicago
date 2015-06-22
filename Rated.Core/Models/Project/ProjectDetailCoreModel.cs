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
        public decimal HoursToComplete { get; set; }
        public string Details { get; set; }
        public decimal Score { get; set; }
        public Guid UserId { get; set; }
        public Guid ReviewerUserId { get; set; }
        public string ReviewerFirstName { get; set; }
        public string ReviewerLastName { get; set; }
        public string ReviewerEmail { get; set; }
        public int ReviewerStatusId { get; set; }
        public int DetailItemNumber { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
