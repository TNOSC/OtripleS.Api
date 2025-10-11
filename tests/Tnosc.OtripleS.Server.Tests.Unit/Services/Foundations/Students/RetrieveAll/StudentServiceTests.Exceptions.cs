// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTests
{
    [Fact]
    public async Task ShouldThrowDependencyExceptionOnRetrieveAllStudentsIfDatabaseUpdateErrorOccursAndLogIt()
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

        _storageBrokerMock.SelectAllStudents()
            .Throws(ex: failedStudentStorageException);

        // when
        ValueTask<IQueryable<Student>> retrieveAllStudentsTask =
           _studentService.RetrieveAllStudentsAsync();

        // then
        await Assert.ThrowsAsync<StudentDependencyException>(() =>
            retrieveAllStudentsTask.AsTask());

        _loggingBrokerMock.Received(1)
            .LogCritical(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentDependencyException)));

        _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .SelectAllStudents();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveAllStudentsIfExceptionOccursAndLogIt()
    {
        // given
        var serviceException = new Exception();

        var failedStudentServiceException =
            new FailedStudentServiceException(
                message: "Failed student service error occurred, contact support.",
                innerException: serviceException);

        var expectedStudentServiceException =
            new StudentServiceException(
                message: "Service error occurred, contact support.",
                innerException: failedStudentServiceException);

        _storageBrokerMock.SelectAllStudents()
            .Throws(ex: serviceException);

        // when
        ValueTask<IQueryable<Student>> retrieveAllStudentsTask =
            _studentService.RetrieveAllStudentsAsync();

        // then
        await Assert.ThrowsAsync<StudentServiceException>(() =>
            retrieveAllStudentsTask.AsTask());

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentServiceException)));

        _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .SelectAllStudents();
    }
}
