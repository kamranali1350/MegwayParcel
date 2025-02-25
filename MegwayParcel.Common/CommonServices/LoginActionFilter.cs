
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MegwayParcel.Common.Data;

namespace MegwayParcel.Common.CommonServices
{
    public class LoginActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string val = context.HttpContext.Session.GetString("Login");
            if (val == null)
            {
                context.Result = new RedirectResult("/Users/Login");
                return;
            }
            else
            {
                User u = JsonConvert.DeserializeObject<User>(val);
                string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName; //Get Controller Name
                bool hasRight = false;
                foreach (var item in u.Role.RoleMenus)
                {
                    if (controllerName == item.Menu.ControllerName)
                    {
                        hasRight = true;
                    }
                }
                if (hasRight == false)
                {
                    context.Result = new RedirectResult("/Users/Login");
                    return;
                }
                //List<RoleMenu> roleMenus = u.Role.RoleMenu.Where(x => x.Menu.ControllerName == controllerName).ToList(); //Linq Query
                //if (roleMenus.Count==0)
                //{
                //    context.Result = new RedirectResult("/Users/Login");
                //    return;
                //}
            }

            //To do : before the action executes  
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
