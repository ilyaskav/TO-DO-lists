using RubyTest.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RubyTest.Models
{
    public class ProjectModel
    {
        
        public int Id { get; set; }

        [Required, StringLength(64, MinimumLength = 2)]
        public string Name { get; set; }

        public ICollection<TaskModel> Tasks { get; set; }

        public ProjectModel()
        {
            Tasks = new List<TaskModel>();
        }
    }
}