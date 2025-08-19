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

public partial class StudentServiceTest
{
    [Fact]
    public async Task ShouldModifyStudentAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        Student inputStudent = randomStudent;
        Student storageStudent = randomStudent;
        Student expectedStudent = storageStudent.DeepClone();

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

        _storageBrokerMock
            .ReceivedCalls()
            .Count()
            .ShouldBe(expected: 1);

        _loggingBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }
}
