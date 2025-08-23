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

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Processings.Students;

public partial class StudentProcessingServiceTests
{
    [Fact]
    public async Task ShouldRegisterStudentIfStudentNotExistAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        Student inputStudent = randomStudent;
        Student registeredStudent = inputStudent;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Student noStudent = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        Student expectedStudent = registeredStudent.DeepClone();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _studentServiceMock
            .RetrieveStudentByIdAsync(studentId: inputStudent.Id)
            .Returns(returnThis: noStudent);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        _studentServiceMock.RegisterStudentAsync(student: inputStudent)
            .Returns(returnThis: registeredStudent);

        // when
        Student actualStudent =
            await _studentProcessingService.UpsertStudentAsync(student: inputStudent);

        // then
        actualStudent.ShouldBeEquivalentTo(expected: expectedStudent);

        await _studentServiceMock
            .Received(requiredNumberOfCalls: 1)
            .RegisterStudentAsync(student: inputStudent);

        await _studentServiceMock
            .Received(requiredNumberOfCalls: 0)
            .ModifyStudentAsync(student: inputStudent);

        _loggingBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }

    [Fact]
    public async Task ShouldModifyStudentIfStudentExistAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        Student inputStudent = randomStudent;
        Student storageStudent = inputStudent;
        Student registeredStudent = inputStudent.DeepClone();
        Student expectedStudent = storageStudent.DeepClone();
        
        _studentServiceMock
            .RetrieveStudentByIdAsync(studentId: inputStudent.Id)
            .Returns(returnThis: registeredStudent);

        _studentServiceMock.ModifyStudentAsync(student: inputStudent)
            .Returns(returnThis: storageStudent);

        // when
        Student actualStudent =
            await _studentProcessingService.UpsertStudentAsync(student: inputStudent);

        // then
        actualStudent.ShouldBeEquivalentTo(expected: expectedStudent);

        await _studentServiceMock
            .Received(requiredNumberOfCalls: 0)
            .RegisterStudentAsync(student: inputStudent);

        await _studentServiceMock
            .Received(requiredNumberOfCalls: 1)
            .ModifyStudentAsync(student: inputStudent);

        _loggingBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();

    }
}
