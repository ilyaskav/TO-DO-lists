using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RubyTest.Models
{
    public class ProjectUpdateModel
    {
        [Required]
        public int ProjectId { get; set; }

        [Required, StringLength(128, MinimumLength=2)]
        public string Name { get; set; }
    }
}