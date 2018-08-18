using System.Web.Mvc;

namespace WebReadOnline.Areas.ReadBookModule
{
    public class ReadBookModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ReadBookModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ReadBookModule_default",
                "ReadBookModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}