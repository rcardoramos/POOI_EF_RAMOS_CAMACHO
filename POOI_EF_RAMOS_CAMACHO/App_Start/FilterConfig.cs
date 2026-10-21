using System.Web;
using System.Web.Mvc;

namespace POOI_EF_RAMOS_CAMACHO
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
