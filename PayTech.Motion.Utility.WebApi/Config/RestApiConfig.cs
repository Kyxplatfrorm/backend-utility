using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayTech.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayTech.Motion.Utility.WebApi
{
    public class RestApiConfig : RestHostStartUpConfig
    {
        public RestApiConfig(IConfiguration Configuration) : base(Configuration)
        {

        }

        public override void InjectConfigureServices(IServiceCollection services)
        {
            
        }

        public override void InjectFilter(IServiceCollection services, FilterCollection filters)
        {
            
        }

        public override List<string> OrigingConfig()
        {
            return new List<string>();
        }

        public override List<RestApiVersionInformation> SetVersionConfig()
        {
            List<RestApiVersionInformation> apiVersionInfos = new();

            RestApiVersionInformation v1 = new()
            {
                Description = "Utility Api v1.0",
                Title = "Utility Api v1.0",
                Version = "v1.0",
                VersionApiName = "Utility Api v1.0"
            };

            apiVersionInfos.Add(v1);


            return apiVersionInfos;
        }
    }
}
