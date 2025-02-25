
using MegwayParcel.Common.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegwayParcel.Web.Controllers
{
    public class ExporterDetailController : Controller
    {
        private readonly LogisticERPContext db;

        public ExporterDetailController(LogisticERPContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Save(ExporterDetail model)
        {
            


            if (model.ExporterDetailId > 0)
            {
                db.ExporterDetails.Add(model);
            }
            else
            {
                db.ExporterDetails.Add(model);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult List()
        {
            
            List<ExporterDetail> lst = db.ExporterDetails.ToList();
            return View(lst);
        }
        public IActionResult Edit(int id)
        {
            

            ExporterDetail edt = db.ExporterDetails.Find(id);
            return View("Index", edt);
        }
    }
}
