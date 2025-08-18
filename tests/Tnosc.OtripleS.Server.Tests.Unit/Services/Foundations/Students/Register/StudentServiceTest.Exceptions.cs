// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Domain.Students.Exceptions;
using Xeptions;
using Xunit;
using EFxceptions.Models.Exceptions;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTest
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRegisterIfSqlErrorOccursAndLogItAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        DateTimeOffset dateTime = randomDateTime;
        Student randomStudent = CreateRandomStudent(date: randomDateTime);
        randomStudent.UpdatedBy = randomStudent.CreatedBy;
        Student inputStudent = randomStudent;
        SqlException sqlException = CreateSqlException(errorCode: GetRandomNumber());

        var failedStudentStorageException =
            new FailedStudentStorageException(
                message: "Failed student storage error occurred, contact support.",
                innerException: sqlException);

        var expectedStudentDependencyException =
            new StudentDependencyException(
                message: "Student dependency error occurred, contact support.",
                innerException: failedStudentStorageException);


        _dateTimeBrokerMock.GetCurrentDateTime()
            .Returns(dateTime);

        _storageBrokerMock.InsertStudentAsync(inputStudent)
               .ThrowsAsync(failedStudentStorageException);

        // when
        ValueTask<Student> registerStudentTask =
            _studentService.RegisterStudentAsync(inputStudent);

        // then
        await Assert.ThrowsAsync<StudentDependencyException>(() =>
            registerStudentTask.AsTask());

        _dateTimeBrokerMock
            .Received(1)
            .GetCurrentDateTime();

        _loggingBrokerMock.Received(1)
            .LogCritical(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentDependencyException)));

        await _storageBrokerMock
            .Received(1)
            .InsertStudentAsync(inputStudent);
    }

    [Fact]
    public async Task ShouldThrowDependencyValidationExceptionOnRegisterWhenStudentAlreadyExistsAndLogItAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        DateTimeOffset dateTime = randomDateTime;
        Student randomStudent = CreateRandomStudent(date: randomDateTime);
        randomStudent.UpdatedBy = randomStudent.CreatedBy;
        Student inputStudent = randomStudent;
        string someMessage = GetRandomMessage();

        var duplicateKeyException =
            new DuplicateKeyException(message: someMessage);

        var alreadyExistsStudentException =
            new AlreadyExistsStudentException(
                message: "Student with the same id already exists.",
                innerException: duplicateKeyException);

        var expectedStudentDependencyValidationException =
            new StudentDependencyValidationException(
                message: "Student dependency validation error occurred, fix the errors and try again.",
                innerException: alreadyExistsStudentException);

        _dateTimeBrokerMock.GetCurrentDateTime()
           .Returns(dateTime);

        _storageBrokerMock.InsertStudentAsync(inputStudent)
               .ThrowsAsync(alreadyExistsStudentException);

        // when
        ValueTask<Student> registerStudentTask =
            _studentService.RegisterStudentAsync(inputStudent);

        // then
        await Assert.ThrowsAsync<StudentDependencyException>(() =>
            registerStudentTask.AsTask());

        _dateTimeBrokerMock
            .Received(1)
            .GetCurrentDateTime();

        _loggingBrokerMock.Received(1)
            .LogCritical(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentDependencyValidationException)));

        await _storageBrokerMock
            .Received(1)
            .InsertStudentAsync(inputStudent);
    }
}
