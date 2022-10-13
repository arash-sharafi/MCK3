using System.Web.Mvc;

namespace Mock3.Areas.ToeflMgt
{
    public class ToeflMgtAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ToeflMgt";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ToeflMgt_default",
                "ToeflMgt/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}