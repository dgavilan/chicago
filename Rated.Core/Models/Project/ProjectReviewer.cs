using Rated.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Models.Project
{
    public class ProjectReviewerCoreModel
    {
        public Guid ProjectReviewerId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ProjectDetailId { get; set; }
        public Guid UserId { get; set; }
        public int StatusId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
