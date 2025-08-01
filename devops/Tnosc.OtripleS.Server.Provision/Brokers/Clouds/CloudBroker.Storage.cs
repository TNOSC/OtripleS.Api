// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using Azure.ResourceManager.Sql.Models;
using Tnosc.OtripleS.Server.Provision.Models;

namespace Tnosc.OtripleS.Server.Provision.Brokers.Clouds;

internal partial class CloudBroker
{
    public async ValueTask<SqlServerResource> CreateSqlServerAsync(
        string sqlServerName,
        ResourceGroupResource resourceGroup)
    {
        SqlServerCollection sqlServerCollection = resourceGroup.GetSqlServers();

        var sqlServerData = new SqlServerData(location: resourceGroup.Data.Location)
        {
            AdministratorLogin = _adminName,
            AdministratorLoginPassword = _adminAccess,
        };

        ArmOperation<SqlServerResource> operation =
            await sqlServerCollection.CreateOrUpdateAsync(
            waitUntil: WaitUntil.Completed,
            serverName: sqlServerName,
            data: sqlServerData);

        return operation.Value;
    }

    public async ValueTask<SqlDatabaseResource> CreateSqlDatabaseAsync(
       string sqlDatabaseName,
       SqlServerResource sqlServer)
    {
        SqlDatabaseCollection databaseCollection = sqlServer.GetSqlDatabases();

        var dbData = new SqlDatabaseData(location: sqlServer.Data.Location)
        {
            Sku = new SqlSku(name: "Basic")
        };

        ArmOperation<SqlDatabaseResource> operation = await databaseCollection.CreateOrUpdateAsync(
            waitUntil: WaitUntil.Completed,
            databaseName: sqlDatabaseName,
            data: dbData);

        return operation.Value;
    }

    public SqlDatabaseAccess GetAdminAccess()
        => new()
        {
            AdminName = _adminName,
            AdminAccess = _adminAccess
        };
}
