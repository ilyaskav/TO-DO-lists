using RubyTest.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RubyTest.Models
{
    public class TaskModel
    {

        public int TaskId { get; set; }

        [Required, StringLength(256, MinimumLength=2)]
        public string Name { get; set; }

        public string Status { get; set; }

        [Required]
        public int ProjectId { get; set; }


        public static explicit operator TaskModel(Task entity){
            return new TaskModel() { TaskId = entity.TaskId, Name = entity.Name, ProjectId = entity.ProjectId, Status = entity.Status };
        } 
    }

    public static class ListExtensions
    {
        public static List<TaskModel> ConvertToModel(this List<Task> entities){
            return entities.ConvertAll(e => (TaskModel)e);
        }
    }
}