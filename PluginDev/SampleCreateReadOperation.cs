using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDev
{
    public class SampleCreateReadOperation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var OrganizationService = serviceFactory.CreateOrganizationService(context.InitiatingUserId);

            if(context.PrimaryEntityName == "account" && context.MessageName == "Create")
            {
                //Read
                Entity accountRecord = OrganizationService.Retrieve(context.PrimaryEntityName, context.PrimaryEntityId, new Microsoft.Xrm.Sdk.Query.ColumnSet("name","telephone1"));
                string accountName = accountRecord.Contains("name") ? accountRecord.GetAttributeValue<string>("name") : "";
                string PhoneNumber = accountRecord.Contains("telephone1") ?  accountRecord.GetAttributeValue<string>("telephone1"): "";
                int customerTypeCode = accountRecord.Contains("customertypecode") ? accountRecord.GetAttributeValue<OptionSetValue>("customertypecode").Value : 0;

                //Create Operation
                Entity contactRecord = new Entity("contact");
                contactRecord["fullname"] = accountName;
                contactRecord["telephone1"] = PhoneNumber;
                contactRecord["parentcustomerid"] = new EntityReference("account", context.PrimaryEntityId);
                contactRecord["accountrolecode"] = new OptionSetValue(2);
                contactRecord["creditlimit"] = new Money(100);
                contactRecord["lastonholdtime"] = new DateTime(2024,09,18);
                contactRecord["donotphone"] = true;
                contactRecord["numberofchildern"] = 0;
                Guid contactId =  OrganizationService.Create(contactRecord);

            }
        }
    }
}
