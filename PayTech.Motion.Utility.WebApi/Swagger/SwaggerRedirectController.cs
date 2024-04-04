using Microsoft.AspNetCore.Mvc;

namespace PayTech.Motion.Admin.WebApi
{
    public class SwaggerRedirectController : Controller
    {
        [Route(""), HttpGet]
        public ActionResult RedirectToSwagger()
        {
            string redirectUrl = "/swagger/index.html";

            var config = PayTech.Core.PayJsonConfigManager.GetServerConfig();
            if (!string.IsNullOrEmpty(config.SwaggerBasePath))
                redirectUrl = "/" + config.SwaggerBasePath + "/swagger/index.html";

            return Redirect(redirectUrl);
        }
    }
}
