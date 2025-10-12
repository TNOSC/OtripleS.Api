// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Diagnostics;

namespace Tnosc.OtripleS.Server.Application.Observabilities;

public static class DiagnosticsConfig
{
    public const string ServiceName = "otriples-api";
    public static readonly ActivitySource ActivitySource = new(ServiceName);
}
