using System.Web;
using System.Web.Mvc;
using EditThor1.Handlers;

namespace EditThor1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
