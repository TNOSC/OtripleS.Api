// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;
using Azure.ResourceManager.Resources;

namespace Tnosc.OtripleS.Server.Provision.Brokers.Clouds;

public partial class CloudBroker
{
    public async ValueTask<WebSiteResource> CreateWebAppAsync(
        string webAppName,
        string databaseConnectionString,
        AppServicePlanResource plan,
        ResourceGroupResource resourceGroup)
    {
        WebSiteCollection webAppCollection = resourceGroup.GetWebSites();

        var siteData = new WebSiteData(resourceGroup.Data.Location)
        {
            AppServicePlanId = plan.Id,
            SiteConfig = new SiteConfigProperties()
            {
                NetFrameworkVersion = "v9.0" ,
                ConnectionStrings = new List<ConnStringInfo>()
                {
                    new ConnStringInfo()
                    {
                        Name = "DefaultConnection",
                        ConnectionString = databaseConnectionString,
                        ConnectionStringType = ConnectionStringType.SqlAzure
                    }
                },
            },
        };

        // Create or update the web app
        ArmOperation<WebSiteResource> operation = await webAppCollection.CreateOrUpdateAsync(
            WaitUntil.Completed,
            webAppName,
            siteData);

        return operation.Value;
    }
}
