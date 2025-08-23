// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTests
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

        _storageBrokerMock.SelectStudentByIdAsync(studentId: inputStudent.Id)
            .Returns(returnThis: beforeUpdateStorageStudent);

        _storageBrokerMock.UpdateStudentAsync(student: inputStudent)
            .ThrowsAsync(ex: failedStudentStorageException);

        // when
        ValueTask<Student> modifyStudentTask =
            _studentService.ModifyStudentAsync(student: inputStudent);

        // then
        await Assert.ThrowsAsync<StudentDependencyException>(() =>
            modifyStudentTask.AsTask());

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

    [Fact]
    public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        Student randomStudent = CreateRandomStudent();
        randomStudent.UpdatedDate = randomDateTime;
        Student inputStudent = randomStudent;
        Student beforeUpdateStorageStudent = randomStudent.DeepClone();
        var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

        var lockedStudentException =
            new LockedStudentException(
                message: "Locked student record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

        var expectedStudentDependencyException =
            new StudentDependencyValidationException(
                message: "Student dependency validation error occurred, fix the errors and try again.",
                innerException: lockedStudentException);

        _dateTimeBrokerMock.GetCurrentDateTime()
            .Returns(returnThis: randomDateTime);

        _storageBrokerMock.SelectStudentByIdAsync(studentId: inputStudent.Id)
            .Returns(returnThis: beforeUpdateStorageStudent);

        _storageBrokerMock.UpdateStudentAsync(student: inputStudent)
            .ThrowsAsync(ex: lockedStudentException);

        // when
        ValueTask<Student> modifyStudentTask =
            _studentService.ModifyStudentAsync(student: inputStudent);

        // then
        await Assert.ThrowsAsync<StudentDependencyValidationException>(() =>
            modifyStudentTask.AsTask());

        _dateTimeBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .GetCurrentDateTime();

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentDependencyException)));

        await _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .SelectStudentByIdAsync(studentId: inputStudent.Id);

        await _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .UpdateStudentAsync(student: inputStudent);
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnModifyIfExceptionOccursAndLogItAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        Student randomStudent = CreateRandomStudent();
        randomStudent.UpdatedDate = randomDateTime;
        Student inputStudent = randomStudent;
        var serviceException = new Exception();

        var failedStudentServiceException =
            new FailedStudentServiceException(
                message: "Failed student service error occurred, contact support.",
                innerException: serviceException);

        var expectedStudentServiceException =
            new StudentServiceException(
                message: "Service error occurred, contact support.",
                innerException: failedStudentServiceException);

        _dateTimeBrokerMock.GetCurrentDateTime()
            .Throws(ex: serviceException);

        // when
        ValueTask<Student> modifyStudentTask =
             _studentService.ModifyStudentAsync(student: inputStudent);

        // then
        await Assert.ThrowsAsync<StudentServiceException>(() =>
            modifyStudentTask.AsTask());

        _dateTimeBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .GetCurrentDateTime();

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentServiceException)));

        _storageBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }
}
