// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace Tnosc.OtripleS.Server.Provision.Brokers.Clouds;

internal partial class CloudBroker
{
    public async ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName) 
    {
        SubscriptionResource subscription = await _armClient.GetDefaultSubscriptionAsync();

        ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();

        bool exists = await resourceGroups.ExistsAsync(resourceGroupName: resourceGroupName);

        return exists;
    }

    public async ValueTask<ResourceGroupResource> CreateResourceGroupAsync(string resourceGroupName)
    {
        SubscriptionResource subscription = await _armClient.GetDefaultSubscriptionAsync();

        ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();

        var resourceGroupData = new ResourceGroupData(location: AzureLocation.WestEurope);

        ArmOperation<ResourceGroupResource> result = await resourceGroups.CreateOrUpdateAsync(
                waitUntil: WaitUntil.Completed,
                resourceGroupName: resourceGroupName,
                data: resourceGroupData
        );

       return result.Value;
    }

    public async ValueTask DeleteResourceGroupAsync(string resourceGroupName)
    {
        SubscriptionResource subscription = await _armClient.GetDefaultSubscriptionAsync();
        ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();

        bool exists = (await resourceGroups.ExistsAsync(resourceGroupName: resourceGroupName)).Value;
        if (!exists)
        {
            return;
        }

        ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(resourceGroupName: resourceGroupName);

        await resourceGroup.DeleteAsync(waitUntil: WaitUntil.Completed);
    }
}

