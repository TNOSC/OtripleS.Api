// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Application.Exceptions.Processings.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Processings.Students;

public partial class StudentProcessingServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnUpsertIfStudentIsNullAndLogItAsync()
    {
        // given
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Student nullStudent = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        var nullStudentProcessingException =
            new NullStudentProcessingException(message: "The student is null.");

        var expectedProcessingStudentValidationException =
           new StudentProcessingValidationException(
               message: "Invalid input, fix the errors and try again.",
               innerException: nullStudentProcessingException);

        // when
#pragma warning disable CS8604 // Possible null reference argument.
        ValueTask<Student> upsertStudentTask =
            _studentProcessingService.UpsertStudentAsync(student: nullStudent);
#pragma warning restore CS8604 // Possible null reference argument.

        // then
        await Assert.ThrowsAsync<StudentProcessingValidationException>(() =>
            upsertStudentTask.AsTask());

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
                actualException.SameExceptionAs(expectedProcessingStudentValidationException)));

        _studentServiceMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnUpsertIfStudentIdIsInvalidAndLogItAsync()
    {
        // given
        var invalidStudent = new Student() { Id = Guid.Empty };

        var invalidStudentProcessingException =
            new InvalidStudentProcessingException(message: "Invalid student. Please fix the errors and try again.");

        invalidStudentProcessingException.AddData(
            key: nameof(Student.Id),
            values: "Id is required");

        var expectedProcessingStudentValidationException =
        new StudentProcessingValidationException(
            message: "Invalid input, fix the errors and try again.",
            innerException: invalidStudentProcessingException);

        // when
        ValueTask<Student> upsertStudentTask =
            _studentProcessingService.UpsertStudentAsync(student: invalidStudent);

        // then
        await Assert.ThrowsAsync<StudentProcessingValidationException>(() =>
            upsertStudentTask.AsTask());

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
                actualException.SameExceptionAs(expectedProcessingStudentValidationException)));

        _studentServiceMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }
}
