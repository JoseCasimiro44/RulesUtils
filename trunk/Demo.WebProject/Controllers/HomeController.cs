using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.WebProject.Models;

namespace Demo.WebProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          HomeModel model = new HomeModel();
          return View(model);
        }
    }
}
