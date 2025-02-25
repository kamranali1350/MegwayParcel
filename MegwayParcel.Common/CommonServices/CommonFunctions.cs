
using MegwayParcel.Common.Data;
using System.Collections.Generic;
using System.Linq;

namespace MegwayParcel.Common.CommonServices
{
    public class CommonFunctions
    {
        private readonly LogisticERPContext db;

        public CommonFunctions(LogisticERPContext context)
        {
            db = context;
        }
        public string MyStaticFunction()
        {
            return "Hello from MyStaticFunction!";
        }
        public List<Page> GetPages()
        {
            //LogisticERPContext db = new();
            var pages = db.Pages.Select(x => new Page { PageId = x.PageId, Title = x.Title, ParentId = x.ParentId, Prefix = x.Prefix, SortOrder = x.SortOrder }).ToList();
            return pages;
        }
    }

}
