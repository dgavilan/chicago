using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Shared
{
    public static class Enums
    {
        public enum ReviewerProjectStatus
        {
            [Description("Review In Progress")]
            InProgress = 500,

            [Description("Review Done")]
            Done = 501
        }

        public enum ProjectStatus
        {
            //[Description("New")]
            //New = 107,

            //[Description("Draft")]
            //OwnerDraft = 102,

            //[Description("Waiting for Reviewer to Accept Review Request")]
            //ReviewerPendingAcceptance = 103,
            
            //[Description("In Progress. Waiting for Project Owner to Finish Project.")]
            //OwnerInProgressWorkingOnProject = 101,

            //[Description("In Progress. Waiting for Reviewer to Finish Review.")]
            //ReviewerInProgressReviewingProject = 106,
            
            //[Description("Project is Done. Project Owner and Reviewer have Completed their tasks.")]
            //Complete = 100,

            [Description("New")]
            New = 100,

            [Description("Draft")]
            Draft = 101,

            [Description("In Progress")]
            InProgress = 102,

            [Description("Done")]
            Done = 103,



            [Description("Waiting for Reviewer to Accept Review Request")]
            ReviewerPendingAcceptance = 1103,

            [Description("In Progress. Waiting for Project Owner to Finish Project.")]
            OwnerInProgressWorkingOnProject = 1101,

            [Description("In Progress. Waiting for Reviewer to Finish Review.")]
            ReviewerInProgressReviewingProject = 1106,

            [Description("Project is Done. Project Owner and Reviewer have Completed their Tasks.")]
            Complete = 1100,
        }

        //public enum ProjectReviewerStatus
        //{
        //    [Description("Needs Reviewer")]
        //    NewTaskNeedsReviewer = 200,

        //    [Description("Waiting for Reviewer to Accept Request")]
        //    WaitingForReviewerToAccept = 201,

        //    [Description("Reviewer Accepted Review Request")]
        //    OwnerIsWorkingOnTask = 202,
            
        //    [Description("Reviewer Declined Review Request")]
        //    ReviewerDeclinedNeedsReviewer = 203
        //}

        public enum UserStatus
        {
            [Description("Reviewer Accepted Review Request")]
            ReviewerAccepted = 300,
            [Description("Waiting for Reviewer to Accept Review Request")]
            PendingReviewerAcceptance = 301
        }

        public enum TaskStatus
        {
            [Description("New")]
            New = 404,

            [Description("Draft")]
            Draft = 400,

            [Description("Pending Reviewer Acceptance")]
            ReviewerPendingAcceptance = 403,

            [Description("In Progress. Waiting for Project Owner to Finish Project.")]
            OwnerInProgressWorkingOnProject = 405,

            [Description("Reviewer is Reviewing Task")]
            ReviewerInProgressReviewingDetail = 401,

            [Description("Review Complete")]
            Done = 402,
       
        }
    }
}
