using MegwayParcel.Common;
using LogisticERP.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegwayParcel.Common.Data;
using MegwayParcel.Common.CommonServices;

namespace MegwayParcel.Web.Controllers
{
    public class CarrierController : Controller
    {
        private readonly LogisticERPContext db;

        public CarrierController(LogisticERPContext context)
        {
            db = context;
        }
        public IActionResult DHL(int id)
        {
            return View();
        }
        public IActionResult DPD()
        {
            return View();
        }
        public IActionResult DX()
        {
            return View();
        }
        public IActionResult Evri()
        {
            return View();
        }
        public IActionResult FDL()
        {
            return View();
        }
        public IActionResult FedEx()
        {
            return View();
        }
        public IActionResult Landmark()
        {
            return View();
        }
        public IActionResult TGEuro()
        {
            return View();
        }
        public IActionResult Page(string id)
        {
            
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");
            if (customer != null)
            {
                //var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.CustomerId == customer.CustomerId && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0)?.Count();
                //ViewBag.basketCount = basketCount ?? 0;

                var guestEmail = customer.GuestUser?.Email;
                var customerId = customer.CustomerId;
                var basketCount = db.Orders.Where(x => (x.IsRemoveFromBasket ?? false) == false && x.OrderStatusId != 5 && x.ConsignmentSummaries.Count > 0
                                && (guestEmail != null ? x.OrderEmail == guestEmail : x.CustomerId == customerId)).Count();
                ViewBag.basketCount = basketCount;
            }
            var result = db.Pages.FirstOrDefault(x => x.Prefix == id);
            return View(result);
        }
    }
}
