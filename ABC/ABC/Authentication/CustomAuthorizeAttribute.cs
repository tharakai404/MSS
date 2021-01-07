using ABC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ABC.Authentication
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        UserServiceImpl userService = new UserServiceImpl();
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            try{
                var userId = Convert.ToString(httpContext.Session["UserId"]);
                if (!string.IsNullOrEmpty(userId))
                {
                    int intID = int.Parse(userId.ToString());
                    var userrols = userService.GetRolesForUser(intID);
                    foreach (var roles in allowedroles)
                    {
                        httpContext.Session["role"] = roles;
                        foreach (var role in userrols)
                        {
                            if (roles.Equals(role.name)) return true;
                        }
                    }
                }
            }
            catch (Exception ex){               
                Console.WriteLine(ex.Message);
                return authorize;
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}