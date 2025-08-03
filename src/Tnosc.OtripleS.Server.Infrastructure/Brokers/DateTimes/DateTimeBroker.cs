// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using Tnosc.OtripleS.Server.Application.Brokers.DateTimes;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.DateTimes;

internal class DateTimeBroker : IDateTimeBroker
{
    public DateTimeOffset GetCurrentDateTime() =>
        DateTimeOffset.UtcNow;
}
