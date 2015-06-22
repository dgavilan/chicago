using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Shared
{
    public static class Enums
    {
        public enum ProjectStatus
        { 
            Completed = 1,
            OnGoing = 2,
            UnSent = 3,
            WaitingApproverAcceptance = 4
        }

        public enum ProjectReviewerStatus
        { 
            Sent = 1,
            Accepted = 2,
            Declined = 3
        }

        public enum UserStatus
        { 
            Complete = 1,
            PendingAcceptance = 2
        }
    }
}
