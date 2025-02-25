
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MegwayParcel.Common;
using System;
using MegwayParcel.Common.Data;
using MegwayParcel.Common.CommonServices;

namespace MegwayParcel.Web.Controllers
{
    public class SettingController : Controller
    {
        private readonly LogisticERPContext db;

        public SettingController(LogisticERPContext context)
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
            return View();
        }
        public IActionResult Save(CompanySetting model)
        {
            if (model.CompanySettingId > 0)
            {
                db.CompanySettings.Update(model);
            }
            else
            {
                db.CompanySettings.Add(model);
            }
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null || customer.Email != "admin@megwayparcels.co.uk")
            {
                return RedirectToAction("Index", "Home");
            }
            ContactCount();
            List<CompanySetting> lst = db.CompanySettings.ToList();
            return View(lst);
        }
        public IActionResult Edit(int id)
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null || customer.Email != "admin@megwayparcels.co.uk")
            {
                return RedirectToAction("Index", "Home");
            }
            ContactCount();
            CompanySetting edt = db.CompanySettings.Find(id);
            return View("Index", edt);
        }
        public IActionResult Order()
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ContactCount();
            var orders = db.Orders.ToList();
            return View(orders);
        }
        public IActionResult Delete(int id)
        {
            var count = db.Customers.Where(x => x.CompanySettingId == id).Count();
            if (count > 0)
            {
                ViewBag.ErrorMessage = "You can't delete this setting because this is in use";
                List<CompanySetting> lst = db.CompanySettings.ToList();
                return View("List", lst);
            }
            else
            {
                var setting = db.CompanySettings.Find(id);
                if (setting != null)
                {
                    db.CompanySettings.Remove(setting);
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }
        }

        public IActionResult Contact()
        {
            ContactCount();
            List<ContactU> lst = db.ContactUs.ToList();
            return View(lst);
        }
        [HttpPost]
        public IActionResult SaveContact(ContactU model)//for save
        {
            try
            {
                model.IsRead = false;
                model.CreatedOn = DateTime.Now;

                db.ContactUs.Add(model);
                db.SaveChanges();
                return Ok(model);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public IActionResult ChangeIsRead(int id, bool isRead = false)
        {
            var contact = db.ContactUs.FirstOrDefault(x => x.ContactUsId == id);
            contact.IsRead = !contact.IsRead;
            db.ContactUs.Update(contact);
            db.SaveChanges();
            return Ok(contact);
        }
        [HttpGet]
        public IActionResult DeleteContact(int id)
        {
            var contact = db.ContactUs.FirstOrDefault(x => x.ContactUsId == id);
            db.ContactUs.Remove(contact);
            db.SaveChanges();
            return Ok(id);
        }

        private void ContactCount()
        {
            var contactCount = db.ContactUs.Where(x => x.IsRead==false)?.Count();
            ViewBag.contactCount = contactCount ?? 0;
        }
    }
}
