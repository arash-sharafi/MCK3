using System.Web.Mvc;

namespace Mock3.Areas.Mgt
{
    public class MgtAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Mgt";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Mgt_default",
                "Mgt/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}