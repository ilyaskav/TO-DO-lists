using RubyTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RubyTest.Controllers.APIs
{
    [Authorize]
    [RoutePrefix("api/task")]
    public class TaskController : ApiController
    {
        private ApplicationDbContext _context = null;

        public TaskController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddTask([FromBody] TaskModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (user == null)
            {
                return BadRequest();
            }

            user.Projects.FirstOrDefault(p => p.ProjectId.Equals(model.ProjectId))
                .Tasks.Add(new RubyTest.Entities.Task
                {
                    Name = model.Name,
                    Status = model.Status,
                    ProjectId = model.ProjectId
                });

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetAllTasks(int? id)
        {
            //int prId;
            //if (!int.TryParse(id, out prId)) BadRequest("Invalid project Id");
            if (id ==null || id < 1) return BadRequest("Wrong list id");

            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (user == null)
            {
                return BadRequest();
            }

            var taskList = user.Projects.FirstOrDefault(p => p.ProjectId.Equals(id))
                .Tasks.ToList();
            List<TaskModel> results = new List<TaskModel>();
            foreach (var listItem in taskList)
            {
                results.Add(new TaskModel()
                {
                    TaskId = listItem.TaskId,
                    Name = listItem.Name,
                    ProjectId = listItem.ProjectId,
                    Status = listItem.Status
                });
            }

            return Json(results);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IHttpActionResult> DeleteTask ([FromBody] TaskDeleteModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (user == null)
            {
                return BadRequest("Invalid account data");
            }

            var task=user.Projects.FirstOrDefault(p => p.ProjectId.Equals(model.ProjectId))
                .Tasks.FirstOrDefault(t => t.TaskId.Equals(model.TaskId) && t.Status.Equals(model.Status));

            if (task == null) return BadRequest("Invavid task data");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> UpdateTask([FromBody] TaskModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (user == null)
            {
                return BadRequest("Invalid account data");
            }

            var task = user.Projects.FirstOrDefault(p => p.ProjectId.Equals(model.ProjectId))
                .Tasks.FirstOrDefault(t => t.TaskId.Equals(model.TaskId) && t.Status.Equals(model.Status));

            if (task == null) return BadRequest("Invavid task data");

            task.Name = model.Name;
            task.Status = model.Status;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("status")]
        public async Task<IHttpActionResult> ChangeTaskStatus([FromBody] TaskStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (user == null)
            {
                return BadRequest("Invalid account data");
            }

            var task = user.Projects.FirstOrDefault(p => p.ProjectId.Equals(model.ProjectId))
                .Tasks.FirstOrDefault(t => t.TaskId.Equals(model.TaskId));

            if (task == null) return BadRequest("Invavid task data");

            task.Status = model.Status;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
