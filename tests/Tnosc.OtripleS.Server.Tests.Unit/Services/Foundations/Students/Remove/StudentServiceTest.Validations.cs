// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Domain.Students.Exceptions;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTest
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveByIdWhenIdIsInvalidAndLogItAsync()
    {
        // given
        Guid inputStudentId = Guid.Empty;

        var invalidStudentException =
          new InvalidStudentException(message: "Invalid student. Please fix the errors and try again.");

        invalidStudentException.AddData(
            key: nameof(Student.Id),
            values: "Id is required");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: invalidStudentException);

        // when
        ValueTask<Student> removeStudentTask =
            _studentService.RemoveStudentByIdAsync(studentId: inputStudentId);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            removeStudentTask.AsTask());

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
                actualException.SameExceptionAs(expectedStudentValidationException)));

        _storageBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();

        _dateTimeBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }


    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnRemoveByIdIfPostIsNotFoundAndLogItAsync()
    {
        //given
        var someStudentId = Guid.NewGuid();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Student noStudent = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        var notFoundStudentException =
            new NotFoundStudentException(message: $"Couldn't find student with id: {someStudentId}.");

        var expectedStudentValidationException =
          new StudentValidationException(
              message: "Invalid input, fix the errors and try again.",
              innerException: notFoundStudentException);

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _storageBrokerMock.SelectStudentByIdAsync(studentId: someStudentId)
          .Returns(returnThis: noStudent);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        //when
        ValueTask<Student> removeStudentTask =
           _studentService.RemoveStudentByIdAsync(studentId: someStudentId);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            removeStudentTask.AsTask());

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
                actualException.SameExceptionAs(expectedStudentValidationException)));

        await _storageBrokerMock
          .Received(requiredNumberOfCalls: 1)
          .SelectStudentByIdAsync(someStudentId);
    }
}
