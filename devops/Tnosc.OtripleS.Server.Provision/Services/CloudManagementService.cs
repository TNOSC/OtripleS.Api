// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using Tnosc.OtripleS.Server.Provision.Brokers.Clouds;
using Tnosc.OtripleS.Server.Provision.Brokers.Loggings;
using Tnosc.OtripleS.Server.Provision.Models.Storages;

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
            await _cloudBroker.CreateResourceGroupAsync(resourceGroupName: resourceGroupName);

        _loggingBroker.LogActivity(message: $"{resourceGroupName} Provisioned.");

        return resourceGroup;
    }

    public async ValueTask<AppServicePlanResource> ProvisionPlanAsync(
        string projectName,
        string environment,
        ResourceGroupResource resourceGroup)
    {
        string planName = $"{projectName}-PLAN-{environment}".ToUpperInvariant();
        _loggingBroker.LogActivity(message: $"Provisioning {planName}...");

        AppServicePlanResource plan =
            await _cloudBroker.CreatePlanAsync(
                planName: planName,
                resourceGroup: resourceGroup);

        _loggingBroker.LogActivity(message: $"{plan} Provisioned");

        return plan;
    }

    public async ValueTask<SqlServerResource> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            ResourceGroupResource resourceGroup)
    {
        string sqlServerName = $"{projectName}-DBSERVER-{environment}".ToUpperInvariant();
        _loggingBroker.LogActivity(message: $"Provisioning {sqlServerName}...");

        SqlServerResource sqlServer =
            await _cloudBroker.CreateSqlServerAsync(
                sqlServerName: sqlServerName,
                resourceGroup: resourceGroup);

        _loggingBroker.LogActivity(message: $"{sqlServer} Provisioned");

        return sqlServer;
    }

    public async ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
           string projectName,
           string environment,
           SqlServerResource sqlServer)
    {
        string sqlDatabaseName = $"{projectName}-db-{environment}".ToLowerInvariant();
        _loggingBroker.LogActivity(message: $"Provisioning {sqlDatabaseName}...");

        SqlDatabaseResource sqlDatabase =
            await _cloudBroker.CreateSqlDatabaseAsync(
                sqlDatabaseName: sqlDatabaseName,
                sqlServer: sqlServer);

        _loggingBroker.LogActivity(message: $"{sqlDatabaseName} Provisioned");

        return new SqlDatabase
        {
            Database = sqlDatabase,
            ConnectionString = GenerateConnectionString(sqlDatabase: sqlDatabase)
        };
    }

    private string GenerateConnectionString(SqlDatabaseResource sqlDatabase)
    {
        SqlDatabaseAccess sqlDatabaseAccess =
            _cloudBroker.GetAdminAccess();

        return $"Server=tcp:{sqlDatabase.Data.Id.Parent!.Name}.database.windows.net,1433;" +
            $"Initial Catalog={sqlDatabase.Data.Name};" +
            $"User ID={sqlDatabaseAccess.AdminName};" +
            $"Password={sqlDatabaseAccess.AdminAccess};";
    }

    public async ValueTask<WebSiteResource> ProvisionWebAppAsync(
            string projectName,
            string environment,
            string databaseConnectionString,
            ResourceGroupResource resourceGroup,
            AppServicePlanResource appServicePlan)
    {
        string webAppName = $"{projectName}-{environment}".ToUpperInvariant();
        _loggingBroker.LogActivity(message: $"Provisioning {webAppName}");

        WebSiteResource webApp =
            await _cloudBroker.CreateWebAppAsync(
                webAppName: webAppName,
                databaseConnectionString: databaseConnectionString,
                plan: appServicePlan,
                resourceGroup: resourceGroup);

       _loggingBroker.LogActivity(message: $"{webAppName} Provisioned");

        return webApp;
    }
}
