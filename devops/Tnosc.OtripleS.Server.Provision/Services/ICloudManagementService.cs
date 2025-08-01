// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;

namespace Tnosc.OtripleS.Server.Provision.Services;

internal interface ICloudManagementService
{
    ValueTask<ResourceGroupResource> ProvisionResourceGroupAsync(
        string projectName,
        string environment);

    ValueTask<AppServicePlanResource> ProvisionPlanAsync(
            string projectName,
            string environment,
            ResourceGroupResource resourceGroup);
}
