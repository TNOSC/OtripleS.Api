// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;

namespace Tnosc.OtripleS.Server.Application.Configurations;

internal sealed class RetryConfig : IRetryConfig
{
    public int MaxRetryAttempts { get; set; } = 3;
    public TimeSpan DelayBetweenFailures { get; set; } = TimeSpan.FromSeconds(2);
}
