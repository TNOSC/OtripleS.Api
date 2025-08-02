// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;
using Azure.ResourceManager.Resources;

namespace Tnosc.OtripleS.Server.Provision.Brokers.Clouds;

internal partial class CloudBroker
{
    public async ValueTask<AppServicePlanResource> CreatePlanAsync(
        string planName,
        ResourceGroupResource resourceGroup)
    {
        AppServicePlanCollection planCollection = resourceGroup.GetAppServicePlans();

        Response<bool> exists = await planCollection.ExistsAsync(name: planName);
        if (exists.Value)
        {
            return await planCollection.GetAsync(name: planName);
        }

        var planData = new AppServicePlanData(location: AzureLocation.WestEurope)
        {
            Sku = new AppServiceSkuDescription
            {
                Name = "S1",
                Tier = "Standard",
                Capacity = 1,
            },
            Kind = "linux",
        };

        ArmOperation<AppServicePlanResource> operation = await planCollection.CreateOrUpdateAsync(
            waitUntil: WaitUntil.Completed,
            name: planName,
            data: planData);

        return operation.Value;
    }
}
