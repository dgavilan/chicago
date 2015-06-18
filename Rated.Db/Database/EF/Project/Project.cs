namespace Rated.Infrastructure.Database.EF.Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Project")]
    public partial class Project
    {
        [Key]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(5000)]
        public string ProjectDescription { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        [Required]
        public int StatusId { get; set; }

        public virtual User User { get; set; }
    }
}
