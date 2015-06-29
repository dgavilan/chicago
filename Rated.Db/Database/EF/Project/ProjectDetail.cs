namespace Rated.Infrastructure.Database.EF.Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectDetail")]
    public partial class ProjectDetail
    {
        [Key]
        public Guid ProjectDetailId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }

        [Required]
        public int DetailNumber { get; set; }

        [Required]
        [StringLength(200)]
        public string DetailName { get; set; }

        [Required]
        [StringLength(5000)]
        public string DetailDescription { get; set; }

        [Required]
        public decimal HoursToComplete { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public int StatusId { get; set; }


        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
