// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
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
    public async Task ShouldModifyStudentAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        Student randomStudent = CreateRandomStudent();
        randomStudent.UpdatedDate = randomDateTime;
        Student inputStudent = randomStudent;
        Student beforeUpdateStorageStudent = randomStudent.DeepClone();
        Student storageStudent = inputStudent;
        Student expectedStudent = storageStudent.DeepClone();

        _dateTimeBrokerMock.GetCurrentDateTime()
            .Returns(returnThis: randomDateTime);

        _storageBrokerMock.SelectStudentByIdAsync(studentId: inputStudent.Id)
            .Returns(returnThis: beforeUpdateStorageStudent);

        _storageBrokerMock.UpdateStudentAsync(student: inputStudent)
            .Returns(returnThis: storageStudent);

        // when
        Student actualStudent =
            await _studentService.ModifyStudentAsync(student: inputStudent);

        // then
        actualStudent.ShouldBeEquivalentTo(expected: expectedStudent);

        await _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .UpdateStudentAsync(student: inputStudent);

        await _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .SelectStudentByIdAsync(studentId: inputStudent.Id);

        _loggingBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }
}
