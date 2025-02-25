
using MegwayParcel.Common.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegwayParcel.Web.Controllers
{
    public class CustomerInvoiceController : Controller
    {
        private readonly LogisticERPContext db;

        public CustomerInvoiceController(LogisticERPContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Save(CustomerInvoice model)
        {
            
            if (model.CustomerInvoiceId>0)
            {
                db.CustomerInvoices.Add(model);
            }
            else
            {
                db.CustomerInvoices.Add(model);
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult List()
        {
            
            List<CustomerInvoice> lst = db.CustomerInvoices.ToList();
            return View(lst);
        }
        public IActionResult Edit(int id)
        {
            

            CustomerInvoice edt = db.CustomerInvoices.Find(id);
            return View("Index", edt);
        }
    }
}
