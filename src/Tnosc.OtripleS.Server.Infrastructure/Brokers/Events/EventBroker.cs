// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using LeVent.Clients;
using Tnosc.OtripleS.Server.Application.Brokers.Events;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Events;

internal sealed partial class EventBroker : IEventBroker
{
    public EventBroker() =>
        StudentEventClient = new LeVentClient<Student>();
}
