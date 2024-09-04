using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDev
{
    public class SampleUpdateOperation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var OrganizationService = serviceFactory.CreateOrganizationService(context.InitiatingUserId);

            if(context.PrimaryEntityName == "account" && context.MessageName == "Update")
            {
                Entity accountRecord = OrganizationService.Retrieve(context.PrimaryEntityName, context.PrimaryEntityId, new Microsoft.Xrm.Sdk.Query.ColumnSet("accountratingcode", "numberofemployee"));
                int accountRating = accountRecord.Contains("accountratingcode") ? accountRecord.GetAttributeValue<OptionSetValue>("accountratingcode").Value : 100;
                int numberOfEmployees = accountRecord.Contains("numberofemployee") ?  accountRecord.GetAttributeValue<OptionSetValue>("numberofemployee").Value : 0;
                Entity accountToUpdate = new Entity("account");
                accountToUpdate.Id = context.PrimaryEntityId;
                if(accountRating == 1 && numberOfEmployees <10)
                {
                    accountToUpdate["revenue"] = new Money(50);

                }
                else
                {
                    accountToUpdate["revenue"] = new Money(100);

                }
                OrganizationService.Update(accountToUpdate);

            }
        }
    }
}
