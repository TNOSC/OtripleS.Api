// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Brokers.Events;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.LocalStudentEvents;

public sealed class LocalStudentEventService : ILocalStudentEventService
{
    private readonly IEventBroker _eventBroker;

    public LocalStudentEventService(IEventBroker eventBroker) =>
        _eventBroker = eventBroker;

    public void ListenToStudentEvent(Func<Student, ValueTask> studentEventHandler) =>
        _eventBroker.ListenToStudentEvent(studentEventHandler);

    public ValueTask PublishStudentAsync(Student student) =>
        throw new NotImplementedException();
}
