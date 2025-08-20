// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Domain.Students.Exceptions;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRetrieveAllStudentsIfDatabaseUpdateErrorOccursAndLogItAsync()
    {
        // given
        var databaseUpdateException = new DbUpdateException();

        var failedStudentStorageException =
            new FailedStudentStorageException(
                message: "Failed student storage error occurred, contact support.",
                innerException: databaseUpdateException);

        var expectedStudentDependencyException =
            new StudentDependencyException(
                message: "Student dependency error occurred, contact support.",
                innerException: failedStudentStorageException);

        _storageBrokerMock.SelectAllStudentsAsync()
            .ThrowsAsync(ex: failedStudentStorageException);

        // when
        ValueTask<IEnumerable<Student>> retrieveAllStudentsTask =
            _studentService.RetrieveAllStudentsAsync();

        // then
        await Assert.ThrowsAsync<StudentDependencyException>(() =>
            retrieveAllStudentsTask.AsTask());

        _loggingBrokerMock.Received(1)
            .LogCritical(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentDependencyException)));

        await _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .SelectAllStudentsAsync();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveAllStudentsIfExceptionOccursAndLogItAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        StudentId studentId = randomStudent.Id;

        var serviceException = new Exception();

        var failedStudentServiceException =
            new FailedStudentServiceException(
                message: "Failed student service error occurred, contact support.",
                innerException: serviceException);

        var expectedStudentServiceException =
            new StudentServiceException(
                message: "Service error occurred, contact support.",
                innerException: failedStudentServiceException);

        _storageBrokerMock.SelectAllStudentsAsync()
            .ThrowsAsync(ex: serviceException);

        // when
        ValueTask<Student> retrieveAllStudentTask =
             _studentService.RemoveStudentByIdAsync(studentId: studentId);

        // then
        await Assert.ThrowsAsync<StudentServiceException>(() =>
            retrieveAllStudentTask.AsTask());

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentServiceException)));

        await _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .SelectAllStudentsAsync();
    }
}
