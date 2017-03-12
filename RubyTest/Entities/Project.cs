using RubyTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RubyTest.Entities
{
    [Table("Projects")]
    public class Project
    {
        #region Properties

        [Key]
        public int ProjectId { get; set; }

        [Required, ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime LastModifiedAt { get; set; }

        #endregion


        // Navigation properties
        public virtual ICollection<Task> Tasks { get; set; }

        public virtual ApplicationUser User { get; set; }


        public Project()
        {
            Tasks = new List<Task>();
        }
    }


}