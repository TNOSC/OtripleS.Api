// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using Azure.Identity;
using Azure.ResourceManager;

namespace Tnosc.OtripleS.Server.Provision.Brokers.Clouds;

internal partial class CloudBroker : ICloudBroker
{
    private readonly ArmClient _armClient;

    public CloudBroker()
    {
        if ( Environment.GetEnvironmentVariable("AZURE_CLIENT_ID") is null)
        {
            throw new InvalidOperationException("AZURE_CLIENT_ID does not exist in environment variables");
        }
        if (Environment.GetEnvironmentVariable("AZURE_TENANT_ID") is null)
        {
            throw new InvalidOperationException("AZURE_TENANT_ID does not exist in environment variables");
        }
        if (Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET") is null)
        {
            throw new InvalidOperationException("AZURE_CLIENT_SECRET does not exist in environment variables");
        }
        _armClient = new ArmClient(new DefaultAzureCredential());
    }
}
