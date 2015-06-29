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
        public enum ProjectStatus
        {
            [Description("Project is Done. Project Owner and Reviewer have Completed their tasks.")]
            Completed = 100,
            [Description("In Progress. Waiting for Project Owner to Finish Project.")]
            InProgressWaitingForOwnerToFinishProject = 101,
            [Description("Draft")]
            Draft = 102,
            [Description("Waiting for Reviewer to Accept Review Request")]
            WaitingApproverAcceptance = 103,
            [Description("In Review")]
            Review = 104,
            [Description("In Progress. Project Owner Finished Project.")]
            InProgressOwnerFinishedProject = 105,
            [Description("In Progress. Waiting for Reviewer to Finish Review.")]
            InProgressWaitingForReviewerToFinishReview = 106,
            [Description("Owner has completed all tasks.")]
            OwnerHasCompletedAllTasks = 107
        }

        public enum ProjectReviewerStatus
        {
            [Description("Needs Reviewer")]
            NeedsReviewer = 200,
            [Description("Sent Review Request to Reviewer")]
            Sent = 201,
            [Description("Reviewer Accepted Review Request")]
            Accepted = 202,
            [Description("Reviewer Declined Review Request")]
            Declined = 203
        }

        public enum UserStatus
        {
            [Description("Reviewer Accepted Review Request")]
            ReviewerAccepted = 300,
            [Description("Waiting for Reviewer to Accept Review Request")]
            PendingReviewerAcceptance = 301
        }

        public enum ProjectDetailStatus
        { 
            [Description("Draft")]
            Draft = 400,
            [Description("Owner is working on task. Reviewer is waiting until task is complete.")]
            InReview = 401,
            [Description("Owner has completed task")]
            OwnerHasCompletedTask = 402,
        }
    }
}
