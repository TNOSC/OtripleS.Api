// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using Tnosc.OtripleS.Server.Provision.Brokers.Configurations;
using Tnosc.OtripleS.Server.Provision.Models.Configurations;
using Tnosc.OtripleS.Server.Provision.Models.Storages;
using Tnosc.OtripleS.Server.Provision.Services;

namespace Tnosc.OtripleS.Server.Provision.Processings;

internal sealed class CloudManagementProcessingService : ICloudManagementProcessingService
{
    private readonly ICloudManagementService _cloudManagementService;
    private readonly IConfigurationBroker _configurationBroker;

    public CloudManagementProcessingService()
    {
        _cloudManagementService = new CloudManagementService();
        _configurationBroker = new ConfigurationBroker();
    }

    public async ValueTask ProcessAsync()
    {
        CloudManagementConfiguration cloudManagementConfiguration =
            _configurationBroker.GetConfigurations();

        await ProvisionAsync(
            projectName: cloudManagementConfiguration.ProjectName,
            cloudAction: cloudManagementConfiguration.Up);

        await DeprovisionAsync(
                projectName: cloudManagementConfiguration.ProjectName,
                cloudAction: cloudManagementConfiguration.Down);
    }

    private async ValueTask ProvisionAsync(
        string projectName,
        CloudAction? cloudAction)
    {
        if (cloudAction is null || string.IsNullOrWhiteSpace(value: projectName))
        {
            return;
        }

        IEnumerable<string> environments = RetrieveEnvironments(cloudAction);

        foreach (string environmentName in environments)
        {
            ResourceGroupResource resourceGroup = await _cloudManagementService
                .ProvisionResourceGroupAsync(
                    projectName: projectName,
                    environment: environmentName);

            AppServicePlanResource appServicePlan = await _cloudManagementService
                .ProvisionPlanAsync(
                    projectName: projectName,
                    environment: environmentName,
                    resourceGroup: resourceGroup);

            SqlServerResource sqlServer = await _cloudManagementService
                .ProvisionSqlServerAsync(
                    projectName: projectName,
                    environment: environmentName,
                    resourceGroup: resourceGroup);

            SqlDatabase sqlDatabase = await _cloudManagementService
                .ProvisionSqlDatabaseAsync(
                    projectName: projectName,
                    environment: environmentName,
                    sqlServer: sqlServer);

            _ = await _cloudManagementService
                .ProvisionWebAppAsync(
                    projectName: projectName,
                    environment: environmentName,
                    databaseConnectionString: sqlDatabase.ConnectionString,
                    resourceGroup: resourceGroup,
                    appServicePlan: appServicePlan);
        }
    }

    private async ValueTask DeprovisionAsync(
            string projectName,
            CloudAction? cloudAction)
    {
        if (cloudAction is null || string.IsNullOrWhiteSpace(value: projectName))
        {
            return;
        }

        IEnumerable<string> environments = RetrieveEnvironments(cloudAction);

        foreach (string environmentName in environments)
        {
            await _cloudManagementService.DeprovisionResouceGroupAsync(
                projectName: projectName,
                environment: environmentName);
        }
    }

    private static IEnumerable<string> RetrieveEnvironments(CloudAction cloudAction) =>
        cloudAction?.Environments ?? [];
}
