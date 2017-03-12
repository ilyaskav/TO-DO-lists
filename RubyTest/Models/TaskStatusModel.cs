using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RubyTest.Models
{
    public class TaskStatusModel
    {
        [Required]
        public int TaskId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required, StringLength(20, MinimumLength=3)]
        public string Status { get; set; }

    }
}