

using Microsoft.AspNetCore.Identity;

namespace MegwayParcel.Common.Data
{
    public class ApplicationRole : IdentityRole
    {
        public string Access { get; set; }
    }
}