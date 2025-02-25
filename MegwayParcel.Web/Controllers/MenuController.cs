
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MegwayParcel.Common;
using MegwayParcel.Common.CommonServices;
using MegwayParcel.Common.Data;

namespace MegwayParcel.Web.Controllers
{
    public class MenuController : Controller
    {

        private readonly LogisticERPContext db;

        public MenuController(LogisticERPContext context)
        {
            db = context;
        }
        [LoginActionFilter]
        public IActionResult Create()
        {

            return View();

        }
        public IActionResult Save(Menu a)
        {
            
            if (a.MenuId > 0)
            {
                db.Menus.Update(a);
            }
            else
            {
                db.Menus.Add(a);
            }

            db.SaveChanges();
            return RedirectToAction("List");
        }
        [LoginActionFilter]
        public IActionResult List()
        {
            
            List<Menu> lst = db.Menus.ToList();
            return View(lst);
        }
        public IActionResult Delete(int id)
        {
            
            Menu del = db.Menus.Find(id);
            db.Menus.Remove(del);
            db.SaveChanges();

            return RedirectToAction("List");
        }
        public IActionResult Edit(int id)
        {
            
            Menu edt = db.Menus.Find(id);
            return View("Create", edt);
        }

    }
}
