using Rated.Core.Shared;
using System;

namespace Rated.Core.Models.Project
{
    public class TaskCoreModel
    {
        private int statusId;

        public Guid TaskId { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal HoursToComplete { get; set; }
        public string Details { get; set; }
        public decimal Score { get; set; }
        public Guid UserId { get; set; }
        public Guid ReviewerUserId { get; set; }
        public string ReviewerFirstName { get; set; }
        public string ReviewerLastName { get; set; }
        public string ReviewerEmail { get; set; }
        public int ReviewerStatusId { get; set; }
        public int TaskItemNumber { get; set; }
        public bool HasReviewer { get; set; }
        public decimal Rating { get; set; }
        public string ReviewerComments { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public string ReviewInstructions { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public int StatusId
        {
            get { return statusId; }
            set
            {
                statusId = value;
                Status = (Enums.TaskStatus)statusId;
            }
        }

        public void MoveToNextStatus()
        {
            switch (this.Status)
            {
                case Enums.TaskStatus.New:
                    this.Status = Enums.TaskStatus.Draft;
                    this.StatusId = (int)Enums.TaskStatus.Draft;
                    break;
                case Enums.TaskStatus.Draft:
                    this.Status = Enums.TaskStatus.ReviewerPendingAcceptance;
                    this.StatusId = (int)Enums.TaskStatus.ReviewerPendingAcceptance;
                    break;
                case Enums.TaskStatus.ReviewerPendingAcceptance:
                    this.Status = Enums.TaskStatus.OwnerInProgressWorkingOnProject;
                    this.StatusId = (int)Enums.TaskStatus.OwnerInProgressWorkingOnProject;
                    break;
                case Enums.TaskStatus.OwnerInProgressWorkingOnProject:
                    this.Status = Enums.TaskStatus.ReviewerInProgressReviewingDetail;
                    this.StatusId = (int)Enums.TaskStatus.ReviewerInProgressReviewingDetail;
                    break;
                case Enums.TaskStatus.ReviewerInProgressReviewingDetail:
                    this.Status = Enums.TaskStatus.Done;
                    this.StatusId = (int)Enums.TaskStatus.Done;
                    break;
            }
        }
    }
}
