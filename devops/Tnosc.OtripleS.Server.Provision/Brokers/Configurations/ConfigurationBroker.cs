// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Tnosc.OtripleS.Server.Provision.Models.Configurations;

namespace Tnosc.OtripleS.Server.Provision.Brokers.Configurations;

internal sealed class ConfigurationBroker : IConfigurationBroker
{
    public CloudManagementConfiguration GetConfigurations()
    {
        IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .SetBasePath(basePath: Directory.GetCurrentDirectory())
            .AddJsonFile(path: "devops/Tnosc.OtripleS.Server.Provision/appSettings.json", optional: false)
            .Build();

        return configurationRoot.Get<CloudManagementConfiguration>() 
            ?? throw new InvalidOperationException(message: $"{nameof(CloudManagementConfiguration)} does not exist.");
    }
}

