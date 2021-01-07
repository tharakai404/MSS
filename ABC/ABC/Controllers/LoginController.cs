using ABC.Models;
using ABC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABC.Controllers
{
    public class LoginController : Controller
    {
        LogService logService = new LogService();
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserDTO model)
        {
            if (ModelState.IsValid)
            {

                if (model != null)
                {
                    //UserData userData = db.UserDatas.Find(id);
                    UserData userData = db.UserDatas.FirstOrDefault(u => u.userid == model.UserId && u.password == model.Password);
                    if (userData==null)
                    {
                        ModelState.AddModelError("", "Invalid User Name or Password");
                        return View(model);
                    }
                    Session["UserName"] = userData.name;
                    Session["UserId"] = userData.id;
                  //  logService.SaveLog("Login", "successfully", Session["UserId"].ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User Name or Password");
                    return View(model);
                }

            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult AppLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AppLogin(UserDTO model)
        {
            if (ModelState.IsValid)
            {

                if (model != null)
                {
                    //UserData userData = db.UserDatas.Find(id);
                    UserData userData = db.UserDatas.FirstOrDefault(u => u.userid == model.UserId && u.password == model.Password);
                    if (userData == null)
                    {
                        ModelState.AddModelError("", "Invalid User Name or Password");
                   //     logService.SaveLog("Login", "faild", model.UserId);
                        return View(model);
                    }
                    Session["UserName"] = userData.name;
                    Session["UserId"] = userData.id;
                    //   logService.SaveLog("Login", "successfully", Session["UserId"].ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User Name or Password");
               ///     logService.SaveLog("Login", "faild", model.UserId);
                    return View(model);
                }

            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserName"] = string.Empty;
            Session["UserId"] = string.Empty;
        //    logService.SaveLog("Logout", "successfully", Session["UserId"].ToString());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CustomerLogin()
        {
            return View();
        }

      /*  [HttpPost]
        public ActionResult CustomerLogin(customer model)
        {


                //UserData userData = db.UserDatas.Find(id);
                customer userData = db..FirstOrDefault(u => u.email == model.mobile || u.mobile == model.mobile);
                if (userData == null)
                {
                    ModelState.AddModelError("", "Invalid Mobile number or Email");
                    return View(model);
                }
                Session["CustomerName"] = userData.name;
                Session["CustomerID"] = userData.id;
                return RedirectToAction("Index", "Cart");


           
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerLogOff()
        {
            Session["CustomerName"] = string.Empty;
            Session["CustomerID"] = string.Empty;
            return RedirectToAction("Index", "Cart");
        }
    }
}