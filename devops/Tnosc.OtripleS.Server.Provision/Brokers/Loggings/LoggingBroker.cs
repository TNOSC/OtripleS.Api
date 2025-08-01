// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;

namespace Tnosc.OtripleS.Server.Provision.Brokers.Loggings;

internal sealed class LoggingBroker : ILoggingBroker
{
    public void LogActivity(string message) =>
        Console.WriteLine(value: message);
}
