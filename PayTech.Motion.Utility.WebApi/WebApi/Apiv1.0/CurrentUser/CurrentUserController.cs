using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace PayTech.Core
{
    [SuppressMessage("Microsoft.Performance", "CA1822")]
    [ApiVersionNeutral]
    [ApiVersion("1.0")]
    [Route("core/coreapi/v{version:apiVersion}/CurrentUser/[action]")]
    public class CurrentUserController : Controller
    {
        [HttpGet()]
        public GetCurrentUserResponse GetCurrentUser()
        {
            GetCurrentUserDbJob dbJob = new();
            return dbJob.Process(new EmptyRequest());
        }

        [HttpPut]
        [PayAuthorizationFilter]
        public GetCurrentUserResponse UpdateCurrentUser([FromBody] UpdateCurrentUserRequest request)
        {
            UpdateCurrentUserDbJob dbJob = new();
            return dbJob.Process(request);
        }

        [HttpPut]
        [PayAuthorizationFilter]
        public ServiceResponse ChangeCurrentUserPassword([FromBody] ChangeCurrentUserPasswordRequest request)
        {
            ChangeCurrentUserPasswordDbJob dbJob = new();
            return dbJob.Process(request);
        }
    }
}