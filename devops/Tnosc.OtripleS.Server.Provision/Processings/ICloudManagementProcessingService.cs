// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace Tnosc.OtripleS.Server.Provision.Processings;

internal interface ICloudManagementProcessingService
{
    ValueTask ProcessAsync();
}
