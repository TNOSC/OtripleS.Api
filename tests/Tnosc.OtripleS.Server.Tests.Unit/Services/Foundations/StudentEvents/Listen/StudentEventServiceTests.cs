// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Application.Brokers.Queues.Messages;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.StudentEvents;

public partial class StudentEventServiceTests
{
    [Fact]
    public async Task ShouldListenToStudentQueueAsync()
    {
        // given
        Func<StudentMessage, ValueTask> studentEventHandlerMock =
            Substitute.For<Func<StudentMessage, ValueTask>>();

        StudentMessage randomStudentMessage = CreateRandomStudentMessage();
        StudentMessage incomingStudentMessage = randomStudentMessage;

        _queueBroker.When(async broker =>
           await broker.ListenToStudentQueueAsync(
               Arg.Any<Func<StudentMessage, ValueTask>>()))
               .Do(async callback =>
                   await callback.Arg<Func<StudentMessage, ValueTask>>()(
                       arg: incomingStudentMessage));

        // when
        await _studentEventService.ListenToStudentEventAsync(
             studentEventHandler: studentEventHandlerMock);

        // then

        await studentEventHandlerMock
             .Received(requiredNumberOfCalls: 1)
             .Invoke(arg: incomingStudentMessage);

        await _queueBroker
            .Received(requiredNumberOfCalls: 1)
            .ListenToStudentQueueAsync(Arg.Is<Func<StudentMessage, ValueTask>>(handler =>
                handler == studentEventHandlerMock));

        studentEventHandlerMock.ReceivedCalls().Count().ShouldBe(expected: 1);
        _queueBroker.ReceivedCalls().Count().ShouldBe(expected: 1);
    }
}
