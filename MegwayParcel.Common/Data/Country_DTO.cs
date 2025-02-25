using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegwayParcel.Common.Data
{
    public class Country
    {
        public int CountryID { get; set; }
        public string Title { get; set; }
        public string CountryCode { get; set; }
    }

    public class CountryRootOutput
    {
        public string Status { get; set; }
        public List<object> Notifications { get; set; }
        public List<Country> Countries { get; set; }
    }

    //-------
    public class Credentials
    {
        public string APIKey { get; set; }
        public string Password { get; set; }
    }

    public class CountryRootInput
    {
        public Credentials Credentials { get; set; }
    }
}
