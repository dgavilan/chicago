using Rated.Core.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Rated.Models.Project
{
    public class TaskViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }
        public int StatusId { get; set; }
        public int TaskCount { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal HoursToComplete { get; set; }
        public string ReviewerFirstName { get; set; }
        public string ReviewerLastName { get; set; }
        public string ReviewerFullName { get; set; }
        public string ReviewerEmail { get; set; }
        public int ReviewerStatusId { get; set; }
        public bool HasReviewer { get; set; }
        public Enums.TaskStatus DetailStatus { get; set; }
        public string ReviewInstructions { get; set; }
        public decimal Rating { get; set; }
        public string ReviewerComments { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public List<ReviewerViewModel> Reviewers { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}