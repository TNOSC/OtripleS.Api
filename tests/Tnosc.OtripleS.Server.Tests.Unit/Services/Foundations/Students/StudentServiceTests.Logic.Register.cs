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
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTests
{
    [Fact]
    public async Task ShouldRegisterStudentAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        DateTimeOffset dateTime = randomDateTime;
        Student randomStudent = CreateRandomStudent(randomDateTime);
        randomStudent.UpdatedBy = randomStudent.CreatedBy;
        Student inputStudent = randomStudent;
        Student storageStudent = randomStudent;
        Student expectedStudent = storageStudent.DeepClone();

        _dateTimeBrokerMock.GetCurrentDateTime().Returns(dateTime);

        _storageBrokerMock.InsertStudentAsync(inputStudent)
                     .Returns(storageStudent);

        // when
        Student actualStudent =
            await _studentService.RegisterStudentAsync(inputStudent);

        // then
        actualStudent.ShouldBe(expectedStudent);

        _dateTimeBrokerMock.Received(1).GetCurrentDateTime();
        await _storageBrokerMock.Received(1).InsertStudentAsync(inputStudent);

        _storageBrokerMock.ReceivedCalls().Count().ShouldBe(1);
        _dateTimeBrokerMock.ReceivedCalls().Count().ShouldBe(1);
        _loggingBrokerMock.ReceivedCalls().ShouldBeEmpty();
    }
}
