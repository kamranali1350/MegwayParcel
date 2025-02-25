using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MegwayParcel.Common.Data;
using MegwayParcel.Common.CommonServices;


namespace MegwayParcel.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly LogisticERPContext db;

        public UserController(LogisticERPContext context)
        {
            db = context;
        }
        [LoginActionFilter]
        public IActionResult Create()
        {
            List<Role> role = GetRoles();
            ViewBag.Roles = new SelectList(role, "RoleId", "RoleName");
            return View();
        }

        [HttpPost]
        public IActionResult Save(User a)
        {
            
            if (a.UserId > 0)
            {
                db.User.Update(a);
            }
            else
            {
                db.User.Add(a);

            }
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public List<Role> GetRoles()
        {
            
            List<Role> lst = db.Role.ToList();
            return lst;
        }
        [LoginActionFilter]
        public IActionResult List()
        {
            
            List<User> lst = db.User.Include(x => x.Role).ToList();

            return View(lst);
        }
        public IActionResult Delete(int id)
        {
            
            User del = db.User.Find(id);
            db.User.Remove(del);
            db.SaveChanges();
            return RedirectToAction("List");

        }
        public IActionResult Edit(int id)
        {
            List<Role> role = GetRoles();
            ViewBag.Roles = new SelectList(role, "RoleId", "RoleName");

            
            User edt = db.User.Find(id);

            return View("Create", edt);
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {

            
            List<User> lst = db.User.Where(a => a.Email == model.Email && a.Password == model.Password)
                                    .Include(x => x.Role).ThenInclude(b => b.RoleMenus).ThenInclude(x => x.Menu).ToList();
            if (lst.Count > 0)
            {
                string s = JsonConvert.SerializeObject(lst.FirstOrDefault(), Formatting.Indented,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                            });

                HttpContext.Session.SetString("Login", s);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Incorrect Email or Password";
                return View();
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("Login");
            return RedirectToAction("Login");
        }
    }
}
