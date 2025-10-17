// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Application.Brokers.Queues.Messages;
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;
using Tnosc.OtripleS.Server.Domain.LibraryCards;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Orchestrations.LibraryAccounts;

public partial class LibraryAccountOrchestrationServiceTests
{
    [Fact]
    public void ShouldListenToStudentEvents()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        Student expectedInputStudent = randomStudent;

        _localStudentEventServiceMock.When(service =>
            service.ListenToStudentEvent(
                Arg.Any<Func<Student, ValueTask>>()))
            .Do(async callback =>
                await callback.Arg<Func<Student, ValueTask>>()(
                    arg: expectedInputStudent));

        // when
        _libraryAccountOrchestrationService
                .ListenToLocalStudentEvent();

        // then
        _localStudentEventServiceMock
            .Received(requiredNumberOfCalls: 1)
               .ListenToStudentEvent(
                   Arg.Any<Func<Student, ValueTask>>());

        _localStudentEventServiceMock
           .ReceivedCalls()
               .Count()
                   .ShouldBe(expected: 1);
    }
}
