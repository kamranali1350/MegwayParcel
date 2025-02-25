using MegwayParcel.Common;
using MegwayParcel.Common.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MegwayParcel.Web.Controllers
{
    public class SiteMapController : Controller
    {
        private readonly LogisticERPContext db;

        public SiteMapController(LogisticERPContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            
            
            var result = db.SiteMaps.OrderBy(x=>x.SortOrder).ToList();
            return View(result);
        }
    }
}
