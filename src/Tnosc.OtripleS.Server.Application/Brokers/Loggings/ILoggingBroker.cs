// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;

namespace Tnosc.OtripleS.Server.Application.Brokers.Loggings;

public interface ILoggingBroker
{
    void LogTrace(string message);
    void LogDebug(string message);
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(Exception exception);
    void LogCritical(Exception exception);
}
