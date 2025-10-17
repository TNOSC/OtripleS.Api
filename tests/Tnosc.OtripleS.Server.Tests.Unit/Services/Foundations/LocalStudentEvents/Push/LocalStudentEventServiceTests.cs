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
    public async Task ShouldPublishStudentAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        Student inputStudent = randomStudent;

        // when
        await _localStudentEventService
            .PublishStudentAsync(inputStudent);

        // then
        await _eventBrokerMock
            .Received(requiredNumberOfCalls: 1)
                .PublishStudentEventAsync(student: inputStudent);

        _eventBrokerMock
            .ReceivedCalls()
                .Count()
                    .ShouldBe(expected: 1);
    }
}
