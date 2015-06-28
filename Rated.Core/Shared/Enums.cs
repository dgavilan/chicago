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
            Completed = 1,
            [Description("In Progress")]
            InProgress = 2,
            [Description("Draft")]
            Draft = 3,
            [Description("Waiting for Approver to Accept Review Request")]
            WaitingApproverAcceptance = 4,
            [Description("In Review")]
            Review = 5
        }

        public enum ProjectReviewerStatus
        {
            [Description("Needs Reviewer")]
            NeedsReviewer = 1,
            [Description("Sent Review Request to Reviewer")]
            Sent = 2,
            [Description("Reviewer Accepted Review Request")]
            Accepted = 3,
            [Description("Reviewer Declined Review Request")]
            Declined = 4
        }

        public enum UserStatus
        {
            [Description("Reviewer Accepted Review Request")]
            Complete = 1,
            [Description("Waiting for Reviewer to Accept Review Request")]
            PendingAcceptance = 2
        }
    }
}
