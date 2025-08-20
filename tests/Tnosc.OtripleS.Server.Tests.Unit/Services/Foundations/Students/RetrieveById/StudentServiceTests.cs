// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

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
    public async Task ShouldRetrieveStudentByIdAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        StudentId studentId = randomStudent.Id;
        Student storageStudent = randomStudent;
        Student expectedStudent = storageStudent.DeepClone();

        _storageBrokerMock.SelectStudentByIdAsync(studentId: studentId)
         .Returns(returnThis: storageStudent);

        // when
        Student actualStudent =
            await _studentService.RetrieveStudentByIdAsync(studentId: studentId);

        // then
        actualStudent.ShouldBeEquivalentTo(expected: expectedStudent);

        await _storageBrokerMock
           .Received(requiredNumberOfCalls: 1)
           .SelectStudentByIdAsync(studentId: studentId);

        _loggingBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }
}
