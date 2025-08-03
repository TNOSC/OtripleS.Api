// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using Microsoft.Extensions.Logging;
using Tnosc.OtripleS.Server.Application.Brokers.Loggings;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Loggings;

internal sealed class LoggingBroker : ILoggingBroker
{
    private readonly ILogger<LoggingBroker> _logger;

    public LoggingBroker(ILogger<LoggingBroker> logger) =>
        _logger = logger;

    public void LogInformation(string message) =>
        _logger.LogInformation(message: message);

    public void LogTrace(string message) =>
        _logger.LogTrace(message: message);

    public void LogDebug(string message) =>
        _logger.LogDebug(message: message);

    public void LogWarning(string message) =>
        _logger.LogWarning(message: message);

    public void LogError(Exception exception) =>
        _logger.LogError(
            exception: exception,
            message: exception.Message);

    public void LogCritical(Exception exception) =>
        _logger.LogCritical(exception: exception,
            message: exception.Message);
}
