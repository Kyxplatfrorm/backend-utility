using PayTech.Core;
using PayTech.Motion.Utility.WebApi;
using System;
using System.Collections.Generic;

namespace PayTech.Motion.Utility.Starter
{
    public class Program
    {
        public static void Main()
        {
            var utcDate = DateTime.UtcNow.Date;

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            var serverConfig = PayJsonConfigManager.GetConfig<ServerConfig>("ServerConfig");
            PayApplicationRunner.SetServiceName(serverConfig.ServiceName, serverConfig.Description);

            var serviceConfig = ApplicationConfigExtension.GetApplicationConfig();
            if (serviceConfig.IsSucceeded == false)
            {
                PayLogger.AddError(serviceConfig.ErrorCode + " " + serviceConfig.ErrorDescription);
                return;
            }

            PayApplicationRunner.SetServiceName(ServerConfigurationManager.ServerConfig.ServiceName, ServerConfigurationManager.ServerConfig.Description);
            ApplicationConfigExtension.InsertApplicationStartLog(ServerConfigurationManager.ServerConfig.ApplicationId);

            List<PayThreadingBase> applications = new();
            RestHostBase<RestApiConfig> webApiHost = new(serviceConfig.RestApiPort);
            RestApplication<RestApiConfig> restApplication = new(webApiHost);
            applications.Add(restApplication);
            PayApplicationRunner.RunApplication(applications, true);
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                ApplicationConfigExtension.InsertApplicationEndLog(ServerConfigurationManager.ServerConfig.ApplicationId);
            }
            catch (Exception)
            {
            }
        }
    }
}
