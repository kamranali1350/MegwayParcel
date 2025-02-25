using MegwayParcel.Admin.Models;
using System.Collections.Generic;

namespace MegwayParcel.Admin.Services
{
    public interface IMvcControllerDiscovery
    {
        IEnumerable<MvcControllerInfo> GetControllers();
    }
}