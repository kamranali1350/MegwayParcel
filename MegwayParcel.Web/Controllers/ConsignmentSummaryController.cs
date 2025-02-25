
using MegwayParcel.Common.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegwayParcel.Web.Controllers
{
    public class ConsignmentSummaryController : Controller
    {
        private readonly LogisticERPContext db;

        public ConsignmentSummaryController(LogisticERPContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Save(ConsignmentSummary model)
        {
            
            if (model.ConsignmentSummaryId > 0)
            {
                db.ConsignmentSummaries.Update(model);
            }
            else
            {
                db.ConsignmentSummaries.Add(model);
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult List()
        {
            
            List<ConsignmentSummary> lst = db.ConsignmentSummaries.ToList();
            return View(lst);
        }
        public IActionResult Edit(int id)
        {
            

            ConsignmentSummary edt = db.ConsignmentSummaries.Find(id);
            return View("Index", edt);
        }


    }
}
