// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)

namespace Tnosc.OtripleS.Server.Provision.Models.Configurations;

internal sealed class CloudManagementConfiguration
{
    public string ProjectName { get; set; } = string.Empty;
    public CloudAction? Up { get; set; } 
    public CloudAction? Down { get; set; }
}
