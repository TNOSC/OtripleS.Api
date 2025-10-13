// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Brokers.Queues;
using Tnosc.OtripleS.Server.Application.Brokers.Queues.Messages;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.StudentEvents;

public class StudentEventService : IStudentEventService
{
    private readonly IQueueBroker _queueBroker;

    public StudentEventService(IQueueBroker queueBroker) =>
        _queueBroker = queueBroker;

    public void ListenToStudentEvent(Func<StudentMessage, ValueTask> studentEventHandler) =>
      throw new NotImplementedException();
}
