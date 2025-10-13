// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Brokers.Queues.Messages;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.StudentEvents;

public interface IStudentEventService
{
    Task ListenToStudentEventAsync(Func<StudentMessage, ValueTask> studentEventHandler);
}
