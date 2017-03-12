using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RubyTest.Entities
{
    [Table("Tasks")]
    public class Task
    {

        #region Properties

        [Key]
        public int TaskId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Status { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        #endregion


        // Navigation propetries

        public virtual Project Project { get; set; }
    }
}