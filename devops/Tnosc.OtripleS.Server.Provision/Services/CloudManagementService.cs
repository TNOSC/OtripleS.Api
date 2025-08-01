// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Azure.ResourceManager.Resources;
using Tnosc.OtripleS.Server.Provision.Brokers.Clouds;
using Tnosc.OtripleS.Server.Provision.Brokers.Loggings;

namespace Tnosc.OtripleS.Server.Provision.Services;

internal sealed class CloudManagementService : ICloudManagementService
{
    private readonly ICloudBroker _cloudBroker;
    private readonly ILoggingBroker _loggingBroker;

    public CloudManagementService()
    {
        _cloudBroker = new CloudBroker();
        _loggingBroker = new LoggingBroker();
    }

    public async ValueTask<ResourceGroupResource> ProvisionResourceGroupAsync(
        string projectName,
        string environment)
    {
        string resourceGroupName = $"{projectName}-RESOURCES-{environment}".ToUpperInvariant();
        _loggingBroker.LogActivity(message: $"Provisioning {resourceGroupName}...");

        ResourceGroupResource resourceGroup =
            await _cloudBroker.CreateResourceGroupAsync(
                resourceGroupName);

        _loggingBroker.LogActivity(message: $"{resourceGroupName} Provisioned.");

        return resourceGroup;
    }
}
