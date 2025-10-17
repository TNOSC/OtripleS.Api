// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LeVent.Clients;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Events;

internal partial class EventBroker
{
    private ILeVentClient<Student> StudentEventClient { get; set; }
    public void ListenToStudentEvent(Func<Student, ValueTask> studentEventHandler) =>
        StudentEventClient.RegisterEventHandler(eventHandler: studentEventHandler);

    public async ValueTask PublishStudentEventAsync(Student student) =>
        await StudentEventClient.PublishEventAsync(@event: student);
}
