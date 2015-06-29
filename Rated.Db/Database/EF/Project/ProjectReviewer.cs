namespace Rated.Infrastructure.Database.EF.Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectReviewer")]
    public partial class ProjectReviewer
    {
        [Key]
        public Guid ProjectReviewerId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ProjectDetailId { get; set; }
        public Guid UserId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int StatusId { get; set; }

        //public virtual User User { get; set; }
        //public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }
    }
}
