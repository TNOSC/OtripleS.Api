﻿// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>", Scope = "type", Target = "~T:Tnosc.OtripleS.Server.Provision.Brokers.Clouds.CloudBroker")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>", Scope = "type", Target = "~T:Tnosc.OtripleS.Server.Provision.Brokers.Clouds.ICloudBroker")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>", Scope = "type", Target = "~T:Tnosc.OtripleS.Server.Provision.Brokers.Loggings.ILoggingBroker")]
[assembly: SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "<Pending>", Scope = "member", Target = "~F:Tnosc.OtripleS.Server.Provision.Services.CloudManagementService._cloudBroker")]
[assembly: SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "<Pending>", Scope = "member", Target = "~F:Tnosc.OtripleS.Server.Provision.Services.CloudManagementService._loggingBroker")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>", Scope = "member", Target = "~M:Tnosc.OtripleS.Server.Provision.Services.CloudManagementService.ProvisionSqlDatabaseAsync(System.String,System.String,Azure.ResourceManager.Sql.SqlServerResource)~System.Threading.Tasks.ValueTask{Azure.ResourceManager.Sql.SqlDatabaseResource}")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>", Scope = "member", Target = "~M:Tnosc.OtripleS.Server.Provision.Services.CloudManagementService.ProvisionSqlDatabaseAsync(System.String,System.String,Azure.ResourceManager.Sql.SqlServerResource)~System.Threading.Tasks.ValueTask{Tnosc.OtripleS.Server.Provision.Models.Storages.SqlDatabase}")]
[assembly: SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "<Pending>", Scope = "member", Target = "~F:Tnosc.OtripleS.Server.Provision.Processings.CloudManagementProcessingService._cloudManagementService")]
[assembly: SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "<Pending>", Scope = "member", Target = "~F:Tnosc.OtripleS.Server.Provision.Processings.CloudManagementProcessingService._configurationBroker")]
[assembly: SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "<Pending>")]
