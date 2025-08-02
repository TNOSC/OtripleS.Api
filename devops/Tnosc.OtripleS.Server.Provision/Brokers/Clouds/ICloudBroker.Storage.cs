// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using Tnosc.OtripleS.Server.Provision.Models.Storages;


namespace Tnosc.OtripleS.Server.Provision.Brokers.Clouds;

internal partial interface ICloudBroker
{
    ValueTask<SqlServerResource> CreateSqlServerAsync(
            string sqlServerName,
            ResourceGroupResource resourceGroup);

    ValueTask<SqlDatabaseResource> CreateSqlDatabaseAsync(
        string sqlDatabaseName,
        SqlServerResource sqlServer);

    SqlDatabaseAccess GetAdminAccess();
}
