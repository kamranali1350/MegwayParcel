using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MegwayParcel.Common;
using MegwayParcel.Common.CommonServices;
using MegwayParcel.Common.Data;

namespace MegwayParcel.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly LogisticERPContext db;

        public RoleController(LogisticERPContext context)
        {
            db = context;
        }
        [LoginActionFilter]
        public IActionResult Create()
        {

            List<Menu> menu = GetMenuDropDown();
            ViewBag.Menu = menu;
            return View();
        }
        public List<Menu> GetMenuDropDown()
        {
            
            //List<DropDownVM> lst = db.Menu.Select(x => new DropDownVM { Id = x.MenuId, Text = x.MenuName }).ToList();
            List<Menu> lst = db.Menus.ToList();
            return lst;
        }
        public IActionResult Save(Role a)
        {
            
            if (a.RoleId > 0)
            {
                List<RoleMenu> roleMenus = db.RoleMenus.Where(b => b.RoleId == a.RoleId).ToList();

                db.RoleMenus.RemoveRange(roleMenus);     //Use for multiple records

                //foreach (var item in roleMenus)
                //{
                //    db.RoleMenu.Remove(item);
                //}

                //for (int i = 0; i < roleMenus.Count; i++)
                //{
                //    db.RoleMenu.Remove(roleMenus[i]);
                //}


                db.Role.Update(a);
            }
            else
            {
                db.Role.Add(a);
            }
            db.SaveChanges();
            return Ok(a);
        }
        [LoginActionFilter]
        public IActionResult List()
        {
            
            List<Role> lst = db.Role.ToList();
            return View(lst);
        }
        public IActionResult Delete(int id)
        {
            
            //Role del = db.Role.Find(id);

            List<RoleMenu> roleMenu = db.RoleMenus.Where(x => x.RoleId == id).ToList();

            db.RoleMenus.RemoveRange(roleMenu);
            Role del = db.Role.Find(id);

            db.Role.Remove(del);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Edit(int id)
        {
            List<Menu> menu = GetMenuDropDown();
            ViewBag.Menu = menu;


            
            //Role edt = db.Role.Find(id);
            Role edt = db.Role.Where(x => x.RoleId == id).Include(a => a.RoleMenus).FirstOrDefault();

            return View("Create", edt);
        }
    }
}
