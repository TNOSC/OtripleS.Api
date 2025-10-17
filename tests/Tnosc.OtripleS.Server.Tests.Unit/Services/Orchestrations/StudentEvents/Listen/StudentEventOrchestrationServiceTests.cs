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
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Orchestrations.StudentEvents;

public partial class StudentEventOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldListenAndAddStudentAsync()
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

        _studentEventServiceMock.When(service =>
            service.ListenToStudentEventAsync(
                Arg.Any<Func<StudentMessage, ValueTask>>()))
            .Do(async callback =>
                await callback.Arg<Func<StudentMessage, ValueTask>>()(
                    arg: incomingStudentMessage));

        _dateTimeBrokerMock
           .GetCurrentDateTime()
               .Returns(returnThis: randomDateTime);
        // when
        await _studentEventOrchestrationService.ListenToStudentEventsAsync();

        // then
        await _studentEventServiceMock
            .Received(requiredNumberOfCalls: 1)
                .ListenToStudentEventAsync(
                    Arg.Any<Func<StudentMessage, ValueTask>>());

        await _localStudentEventServiced
           .Received(requiredNumberOfCalls: 1)
               .PublishStudentAsync(
                  Arg.Is<Student>(student =>
                     SameStudentAs(student, expectedInputStudent)));

        Received.InOrder(async () =>
        {
            await _studentServiceMock.RegisterStudentAsync(
                student: Arg.Any<Student>());

            await _localStudentEventServiced.PublishStudentAsync(
                student: Arg.Any<Student>());
        });

        _studentEventServiceMock
            .ReceivedCalls()
                .Count()
                    .ShouldBe(expected: 1);

        _localStudentEventServiced
            .ReceivedCalls()
                .Count()
                    .ShouldBe(expected: 1);
    }
}
