
using MegwayParcel.Common.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegwayParcel.Web.Controllers
{
    public class DeliveryDetailController : Controller
    {
        private readonly LogisticERPContext db;

        public DeliveryDetailController(LogisticERPContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Save(DeliveryDetail model)
        {
            


            if (model.DeliveryDetailId > 0)
            {
                db.DeliveryDetails.Add(model);
            }
            else
            {
                db.DeliveryDetails.Add(model);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult List()
        {
            
            List<DeliveryDetail> lst = db.DeliveryDetails.ToList();
            return View(lst);
        }
        public IActionResult Edit(int id)
        {
            

            DeliveryDetail edt = db.DeliveryDetails.Find(id);
            return View("Index", edt);
        }
    }
}
