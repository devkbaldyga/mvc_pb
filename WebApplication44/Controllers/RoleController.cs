using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication44.Models;

namespace WebApplication44.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public string Create()
        {
            IdentityManager im = new IdentityManager();

            im.CreateRole("admin");
            im.CreateRole("user");

            return "Role utworzone";
        }

        public string AddToRole()
        {
            IdentityManager im = new IdentityManager();

            im.AddUserToRoleByUsername("m.kopczynski@pb.edu.pl", "admin");

            return "Przypisanie do roli utworzone";
        }
    }
}