using Rated.Core.Shared;
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
        public bool HasReviewer { get; set; }
        public decimal DetailRating { get; set; }
        public string ReviewerComments { get; set; }
        
        //public int StatusId { get; set; }
        private int statusId;
        public int StatusId
        {
            get { return statusId; }
            set
            {
                statusId = value;
                DetailStatus = (Enums.ProjectDetailStatus)statusId;
            }
        }

        public Enums.ProjectDetailStatus DetailStatus { get; set; }
        public string ReviewInstructions { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public void MoveToNextStatus()
        {
            switch (this.DetailStatus)
            {
                case Enums.ProjectDetailStatus.New:
                    this.DetailStatus = Enums.ProjectDetailStatus.Draft;
                    this.StatusId = (int)Enums.ProjectDetailStatus.Draft;
                    break;
                case Enums.ProjectDetailStatus.Draft:
                    this.DetailStatus = Enums.ProjectDetailStatus.ReviewerPendingAcceptance;
                    this.StatusId = (int)Enums.ProjectDetailStatus.ReviewerPendingAcceptance;
                    break;
                case Enums.ProjectDetailStatus.ReviewerPendingAcceptance:
                    this.DetailStatus = Enums.ProjectDetailStatus.OwnerInProgressWorkingOnProject;
                    this.StatusId = (int)Enums.ProjectDetailStatus.OwnerInProgressWorkingOnProject;
                    break;
                case Enums.ProjectDetailStatus.OwnerInProgressWorkingOnProject:
                    this.DetailStatus = Enums.ProjectDetailStatus.ReviewerInProgressReviewingDetail;
                    this.StatusId = (int)Enums.ProjectDetailStatus.ReviewerInProgressReviewingDetail;
                    break;
                case Enums.ProjectDetailStatus.ReviewerInProgressReviewingDetail:
                    this.DetailStatus = Enums.ProjectDetailStatus.Done;
                    this.StatusId = (int)Enums.ProjectDetailStatus.Done;
                    break;
            }
        }
    }
}
