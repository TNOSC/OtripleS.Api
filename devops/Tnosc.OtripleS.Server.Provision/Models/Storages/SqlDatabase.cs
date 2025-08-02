// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Azure.ResourceManager.Sql;

namespace Tnosc.OtripleS.Server.Provision.Models.Storages;

internal sealed class SqlDatabase
{
    public string ConnectionString { get; set; } = null!;
    public SqlDatabaseResource Database { get; set; } = null!;
}
