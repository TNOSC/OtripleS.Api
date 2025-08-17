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
               .ThrowsAsync(sqlException);

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
            .LogError(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentDependencyException)));

        await _storageBrokerMock
            .Received(1)
            .InsertStudentAsync(inputStudent);
    }
}
