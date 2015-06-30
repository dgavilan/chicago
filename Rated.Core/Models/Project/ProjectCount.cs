using Rated.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Models.Project
{
    public class ProjectCount
    {
        public int ProjectDraft { get; set; }
        public int ProjectPending { get; set; }
        public int ProjectInProgress { get; set; }
        public int ProjectComplete { get; set; }

        public int ReviewPending { get; set; }
        public int ReviewInProgress { get; set; }
        public int ReviewComplete { get; set; }
    }
}
