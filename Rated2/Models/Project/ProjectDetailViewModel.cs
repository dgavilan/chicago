using Rated.Core.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Rated.Models.Project
{
    public class ProjectDetailViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid ProjectDetailId { get; set; }
        public int DetailCount { get; set; }
        public string DetailName { get; set; }
        public decimal HoursToComplete { get; set; }
        public string DetailDescription { get; set; }
        public string ReviewerFirstName { get; set; }
        public string ReviewerLastName { get; set; }
        public string ReviewerEmail { get; set; }
        public int ReviewerStatusId { get; set; }
        //public Enums.ProjectReviewerStatus ReviewerStatus { get; set; }
        public string ReviewerFullName { get; set; }
        public bool HasReviewer { get; set; }
        public int StatusId { get; set; }
        public Enums.ProjectDetailStatus DetailStatus { get; set; }
        public string ReviewInstructions { get; set; }
        public decimal DetailRating { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}