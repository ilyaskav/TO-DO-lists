using RubyTest.Entities;
using RubyTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RubyTest.Controllers.APIs
{
    [Authorize]
    [RoutePrefix("api/list")]
    public class ListController : ApiController
    {
        private ApplicationDbContext _context = null;

        public ListController()
        {
            _context = new ApplicationDbContext();
        }


        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddList()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (user == null)
            {
                return BadRequest();
            }
            Project NewProject = new Project() { UserId = user.Id, Name = "New List", LastModifiedAt=DateTime.Now };

            user.Projects.Add(NewProject);

            await _context.SaveChangesAsync();
            //var id = NewProject.ProjectId;
            return Ok();
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(ProjectModel))]
        public IHttpActionResult GetLists()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (user == null)
            {
                return BadRequest();
            }

            var projects = _context.Projects.Where(p => p.User.Id.Equals(user.Id)).AsEnumerable().OrderBy(p => p.LastModifiedAt).ToList();
            var results = new List<ProjectModel>();
            foreach (var item in projects)
            {
                results.Add(new ProjectModel()
                {
                    Id = item.ProjectId,
                    Name = item.Name,
                    Tasks = (item.Tasks as List<RubyTest.Entities.Task>).ConvertToModel()//item.Tasks //new List<TaskModel>() {
                });
            }

            return Json(results);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteList([FromUri] int? id)
        {
            if (id == null || id < 1) return BadRequest("Wrong list id");

            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }

            var list = user.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (list == null) return BadRequest("No list with this id found");

            _context.Projects.Remove(list);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> UpdateList([FromBody] ProjectUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }

            var list = user.Projects.FirstOrDefault(p => p.ProjectId.Equals(model.ProjectId));

            if (list == null) return BadRequest();

            list.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
