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
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Coordinations.StudentEvents;

public partial class StudentEventCoordinationServiceTests
{
    [Fact]
    public async Task ShouldListenToStudentEventsAsync()
    {
        // given
        Student expectedInputStudent = CreateRandomStudent();

        var expectedInputLibraryAccount = new LibraryAccount
        {
            Id = Guid.NewGuid(),
            StudentId = expectedInputStudent.Id
        };

       _studentEventOrchestrationServiceMock.When(service =>
            service.ListenToStudentEventsAsync(
                Arg.Any<Func<Student, ValueTask>>()))
            .Do(async callback =>
                await callback.Arg<Func<Student, ValueTask>>()(
                    arg: expectedInputStudent));

        // when
       await _studentEventCoordinationService.ListenToStudentEventsAsync();

        // then
        await _studentEventOrchestrationServiceMock
          .Received(requiredNumberOfCalls: 1)
              .ListenToStudentEventsAsync(
                  Arg.Any<Func<Student, ValueTask>>());

        await _libraryAccountOrchestrationServiceMock
                 .Received(requiredNumberOfCalls: 1)
                     .CreateLibraryAccountAsync(
                         Arg.Is(SameLibraryAccountAs(
                            expectedInputLibraryAccount)));

        _studentEventOrchestrationServiceMock
            .ReceivedCalls()
                .Count()
                    .ShouldBe(expected: 1);

        _libraryAccountOrchestrationServiceMock
            .ReceivedCalls()
                .Count()
                    .ShouldBe(expected: 1);
    }
}
