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
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.LocalStudentEvents;

public partial class LocalStudentEventServiceTests
{
    [Fact]
    public void ShouldListenToStudentEvent()
    {
        // given
        Func<Student, ValueTask> studentEventHandlerMock =
            Substitute.For<Func<Student, ValueTask>>();

        // when
        _localStudentEventService.ListenToStudentEvent(
            studentEventHandlerMock);

        // then
        _eventBrokerMock
            .Received(requiredNumberOfCalls: 1)
                .ListenToStudentEvent(studentEventHandlerMock);

        _eventBrokerMock
            .ReceivedCalls()
                .Count()
                    .ShouldBe(expected: 1);
    }
}
