using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDev
{
    public class SasmpleCreateReadOperation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var OrganizationService = serviceFactory.CreateOrganizationService(context.InitiatingUserId);

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                if (entity.LogicalName == "account")
                {
                    TracingService.Trace("Account Entity is being created");
                    var accountName = entity.Attributes["name"].ToString();
                    TracingService.Trace("Account Name: " + accountName);
                    var accountId = OrganizationService.Create(entity);
                    TracingService.Trace("Account Created with Id: " + accountId);
                    var account = OrganizationService.Retrieve("account", accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                    TracingService.Trace("Account Retrieved with Id: " + account.Id);
                }
            }
        }
    }
}
