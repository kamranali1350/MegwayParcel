using MegwayParcel.Common.CommonServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MegwayParcel.Common;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using MegwayParcel.Common.Data;

namespace MegwayParcel.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly LogisticERPContext db;

        public PageController(LogisticERPContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null || customer.Email != "admin@megwayparcels.co.uk")
            {
                return RedirectToAction("Index", "Home");
            }
            ContactCount();
            List<Page> lst = db.Pages.ToList();
            return View(lst);
        }
        public IActionResult Create()
        {
            
            var pages = db.Pages.Select(x => new Page { PageId = x.PageId, Title = x.Title }).ToList();
            ViewBag.pageDropdown = new SelectList(pages, "PageId", "Title");
            return View();
        }

        public IActionResult Save(Page a)
        {
            
            if (a.PageId > 0)
            {
                db.Pages.Update(a);
            }
            else
            {
                db.Pages.Add(a);
            }

            db.SaveChanges();
            return Ok(a);
        }
        public IActionResult Delete(int id)
        {
            
            Page del = db.Pages.Find(id);
            db.Pages.Remove(del);
            db.SaveChanges();

            return RedirectToAction("List");
        }
        public IActionResult Edit(int id)
        {
            
            var pages = db.Pages.Where(x => x.PageId != id).Select(x => new Page { PageId = x.PageId, Title = x.Title }).ToList();
            ViewBag.pageDropdown = new SelectList(pages, "PageId", "Title");
            Page edt = db.Pages.Find(id);
            return View("Create", edt);
        }
        private void ContactCount()
        {
            var contactCount = db.ContactUs.Where(x => x.IsRead == false)?.Count();
            ViewBag.contactCount = contactCount ?? 0;
        }

       
    }
}
