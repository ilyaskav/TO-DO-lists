using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RubyTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return View();
            else return Redirect("/Home/Demo");
        }

        public ActionResult Demo()
        {
            return View();
        }
    }
}