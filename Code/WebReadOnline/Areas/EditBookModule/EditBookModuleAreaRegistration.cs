using System.Web.Mvc;

namespace WebReadOnline.Areas.EditBookModule
{
    public class EditBookModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EditBookModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EditBookModule_default",
                "EditBookModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}