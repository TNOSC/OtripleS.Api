// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Domain.Students.Exceptions;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTest
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        Student randomStudent = CreateRandomStudent();
        randomStudent.UpdatedDate = randomDateTime;
        Student inputStudent = randomStudent;
        Student beforeUpdateStorageStudent = randomStudent.DeepClone();

        _dateTimeBrokerMock.GetCurrentDateTime()
            .Returns(returnThis: randomDateTime);

        _storageBrokerMock.SelectStudentByIdAsync(studentId: inputStudent.Id)
         .Returns(returnThis: beforeUpdateStorageStudent);

        var databaseUpdateException = new DbUpdateException();

        var failedStudentStorageException =
            new FailedStudentStorageException(
                message: "Failed student storage error occurred, contact support.",
                innerException: databaseUpdateException);

        var expectedStudentDependencyException =
            new StudentDependencyException(
                message: "Student dependency error occurred, contact support.",
                innerException: failedStudentStorageException);

        _dateTimeBrokerMock.GetCurrentDateTime()
            .Returns(returnThis: randomDateTime);

        _storageBrokerMock.UpdateStudentAsync(student: inputStudent)
            .ThrowsAsync(ex: failedStudentStorageException);

        // when
        ValueTask<Student> registerStudentTask =
            _studentService.ModifyStudentAsync(student: inputStudent);

        // then
        await Assert.ThrowsAsync<StudentDependencyException>(() =>
            registerStudentTask.AsTask());

        _dateTimeBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .GetCurrentDateTime();

        _loggingBrokerMock.Received(1)
            .LogCritical(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentDependencyException)));

        await _storageBrokerMock
           .Received(requiredNumberOfCalls: 1)
           .SelectStudentByIdAsync(studentId: inputStudent.Id);

        await _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .UpdateStudentAsync(student: inputStudent);
    }
}
