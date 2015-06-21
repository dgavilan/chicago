﻿using Rated.Core.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Rated2.Models.Project
{
    public class ProjectDetailViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid ProjectDetailId { get; set; }
        public int DetailCount { get; set; }
        public string DetailName { get; set; }
        public decimal HoursToComplete { get; set; }
        public string DetailDescription { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}