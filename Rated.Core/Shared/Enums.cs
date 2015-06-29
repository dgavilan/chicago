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
            [Description("Completed Project")]
            Completed = 100,
            [Description("In Progress")]
            InProgress = 101,
            [Description("Draft")]
            Draft = 102,
            [Description("Waiting for Approver to Accept Review Request")]
            WaitingApproverAcceptance = 103,
            [Description("In Review")]
            Review = 104
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
    }
}
