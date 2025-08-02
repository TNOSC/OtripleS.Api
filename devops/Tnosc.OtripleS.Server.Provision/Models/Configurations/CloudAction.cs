// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)

using System.Collections.Generic;

namespace Tnosc.OtripleS.Server.Provision.Models.Configurations;

internal sealed class CloudAction
{
    public IEnumerable<string> Environments { get; set; } = [];
}
