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

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Orchestrations.StudentEvents;

public partial class StudentEventOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldListenAndAddStudent()
    {
        // given
        StudentMessage randomStudentMessage = CreateRandomStudentMessage();
        StudentMessage incomingStudentMessage = randomStudentMessage;

        _studentEventServiceMock.When(service =>
            service.ListenToStudentEventAsync(
                Arg.Any<Func<StudentMessage, ValueTask>>()))
            .Do(async callback =>
                await callback.Arg<Func<StudentMessage, ValueTask>>()(
                    incomingStudentMessage));

        // when
        _studentEventOrchestrationService.ListenToStudentEvents();

        // then
        await _studentEventServiceMock
            .Received(requiredNumberOfCalls: 1)
            .ListenToStudentEventAsync(Arg.Any<Func<StudentMessage, ValueTask>>());

        _studentEventServiceMock.ReceivedCalls().Count().ShouldBe(1);
        _studentServiceMock.ReceivedCalls().Count().ShouldBe(1);
    }
}
