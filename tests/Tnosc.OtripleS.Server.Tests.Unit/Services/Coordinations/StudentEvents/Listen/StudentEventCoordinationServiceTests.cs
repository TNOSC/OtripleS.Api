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
        var randomUserId = Guid.NewGuid();
        DateTimeOffset randomDateTime = GetRandomDate();

        dynamic randomStudentProperties =
            CreateRandomStudentProperties(
                auditDates: randomDateTime,
                auditIds: randomUserId);

        var randomStudentMessage = new StudentMessage
        {
            IdentityNumber = randomStudentProperties.IdentityNumber,
            FirstName = randomStudentProperties.FirstName,
            MiddleName = randomStudentProperties.MiddleName,
            LastName = randomStudentProperties.LastName,
            Gender = randomStudentProperties.GenderMessage,
            BirthDate = randomStudentProperties.BirthDate,
            CreatedBy = randomUserId,
            UpdatedBy = randomUserId
        };
        StudentMessage incomingStudentMessage = randomStudentMessage;

        var randomStudent = new Student
        {
            Id = randomStudentProperties.Id,
            UserId = randomStudentProperties.UserId,
            IdentityNumber = randomStudentProperties.IdentityNumber,
            FirstName = randomStudentProperties.FirstName,
            MiddleName = randomStudentProperties.MiddleName,
            LastName = randomStudentProperties.LastName,
            Gender = randomStudentProperties.Gender,
            BirthDate = randomStudentProperties.BirthDate,
            CreatedDate = randomDateTime,
            UpdatedDate = randomDateTime,
            CreatedBy = randomUserId,
            UpdatedBy = randomUserId
        };
        Student expectedInputStudent = randomStudent;

        var expectedInputLibraryAccount = new LibraryAccount
        {
            Id = Guid.NewGuid(),
            StudentId = expectedInputStudent.Id
        };

       _studentEventOrchestrationServiceMock.When(service =>
            service.ListenToStudentEventsAsync(
                Arg.Any<Func<StudentMessage, ValueTask>>()))
            .Do(async callback =>
                await callback.Arg<Func<StudentMessage, ValueTask>>()(
                    arg: incomingStudentMessage));

        // when
       await _studentEventCoordinationService.ListenToStudentEventsAsync();

        // then
        await _studentEventOrchestrationServiceMock
          .Received(requiredNumberOfCalls: 1)
              .ListenToStudentEventsAsync(
                  Arg.Any<Func<StudentMessage, ValueTask>>());

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
