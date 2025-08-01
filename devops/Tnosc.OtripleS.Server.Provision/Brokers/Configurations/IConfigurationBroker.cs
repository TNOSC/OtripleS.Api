// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Tnosc.OtripleS.Server.Provision.Models.Configurations;

namespace Tnosc.OtripleS.Server.Provision.Brokers.Configurations;

internal interface IConfigurationBroker
{
    CloudManagementConfiguration GetConfigurations();
}
