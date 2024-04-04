using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace PayTech.Core
{
    [SuppressMessage("Microsoft.Performance", "CA1822")]
    [ApiVersionNeutral]
    [ApiVersion("1.0")]
    [Route("core/coreapi/v{version:apiVersion}/Authentication/[action]")]
    public class AuthenticationController : Controller
    {
        [HttpPost]
        public UserLoginResponse Login([FromBody] UserLoginRequest request)
        {
            UserLoginDb dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPost]
        [PayAuthorizationFilter(true)]
        public VerifyLoginOtpResponse VerifyLoginOtp([FromBody] VerifyLoginOtpRequest request)
        {
            VerifyLoginOtpDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpGet]
        [PayAuthorizationFilter]
        public ServiceResponse Logout()
        {
            UserLogoutDb dbJob = new();
            return dbJob.Process(new EmptyRequest());
        }

        [HttpPut]
        [PayAuthorizationFilter(true)]
        public ServiceResponse ForceChangePassword([FromBody] ForceChangePasswordRequest request)
        {
            ForceChangePasswordDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpGet]
        [PayAuthorizationFilter(true)]
        public GetUserMenuTreeResponse GetUserMenuTree()
        {
            GetUserMenuTreeRequest getUserMenuTreeRequest = new GetUserMenuTreeRequest();
            getUserMenuTreeRequest.ProductModuleId = ServerConfigurationManager.ServerConfig.ProductModuleId;
            getUserMenuTreeRequest.UserId = PaySessionExtension.PaySession.UserId;
            GetUserMenuTreeDbJob dbJob = new();
            return dbJob.Process(getUserMenuTreeRequest);
        }

        [HttpGet]
        [PayAuthorizationFilter]
        public ServiceResponse VerifySession()
        {
            VerifySessionDbJob dbJob = new();
            return dbJob.Process(new EmptyRequest());
        }
    }
}
