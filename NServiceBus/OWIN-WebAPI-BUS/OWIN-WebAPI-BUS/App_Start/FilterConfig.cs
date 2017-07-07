using System.Web;
using System.Web.Mvc;

namespace OWIN_WebAPI_BUS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
