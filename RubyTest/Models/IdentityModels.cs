using Microsoft.AspNet.Identity.EntityFramework;
using RubyTest.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace RubyTest.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Project> Projects { get; set; }

        public ApplicationUser()
        {
            Projects = new List<Project>();
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}