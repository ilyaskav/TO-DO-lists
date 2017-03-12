using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RubyTest.Models
{
    public class TaskDeleteModel
    {
        [Required]
        public int TaskId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string Status { get; set; }
    }
}