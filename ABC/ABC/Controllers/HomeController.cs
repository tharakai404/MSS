using ABC.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABC.Controllers
{
    [CustomAuthenticationFilter]
    public class HomeController : Controller
    {
        public ActionResult UnAuthorized()
        {
           string role = Session["role"].ToString();
            ViewBag.Message = "required role name : " + role;

            return View();
        }

        //[CustomAuthorize("Normal", "Admin")]
        public ActionResult Index()
        {
            return View();
        }
       // [CustomAuthorize("Admin", "SuperAdmin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}