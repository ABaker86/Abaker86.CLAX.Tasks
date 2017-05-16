using System.Web;
using System.Web.Mvc;

namespace Abaker86.CLAX.Tasks
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
