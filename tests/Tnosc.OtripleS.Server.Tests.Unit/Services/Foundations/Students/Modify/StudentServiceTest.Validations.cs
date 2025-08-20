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
    public async Task ShouldThrowValidationExceptionOnModifyWhenStudentIsNullAndLogItAsync()
    {
        // given
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Student invalidStudent = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        var nullStudentException = new NullStudentException(message: "The student is null.");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: nullStudentException);

        // when
#pragma warning disable CS8604 // Possible null reference argument.
        ValueTask<Student> modifyStudentTask =
            _studentService.ModifyStudentAsync(student: invalidStudent);
#pragma warning restore CS8604 // Possible null reference argument.

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            modifyStudentTask.AsTask());

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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ShouldThrowValidationExceptionOnModifyWhenStudentIsInvalidAndLogItAsync(string? invalidText)
    {
        // given
#pragma warning disable CS8601 // Possible null reference assignment.
        var invalidStudent = new Student(id: new StudentId(Guid.Empty))
        {
            UserId = invalidText,
            IdentityNumber = invalidText,
            FirstName = invalidText,
            LastName = invalidText,
        };
#pragma warning restore CS8601 // Possible null reference assignment.

        var invalidStudentException =
            new InvalidStudentException(message: "Invalid student. Please fix the errors and try again.");

        invalidStudentException.AddData(
            key: nameof(Student.Id),
            values: "Id is required");

        invalidStudentException.AddData(
            key: nameof(Student.UserId),
            values: "Text is required");

        invalidStudentException.AddData(
            key: nameof(Student.IdentityNumber),
            values: "Text is required");

        invalidStudentException.AddData(
            key: nameof(Student.FirstName),
            values: "Text is required");

        invalidStudentException.AddData(
            key: nameof(Student.LastName),
            values: "Text is required");

        invalidStudentException.AddData(
            key: nameof(Student.BirthDate),
            values: "Date is required");

        invalidStudentException.AddData(
            key: nameof(Student.CreatedBy),
            values: "Id is required");

        invalidStudentException.AddData(
            key: nameof(Student.CreatedDate),
            values: "Date is required");

        invalidStudentException.AddData(
            key: nameof(Student.UpdatedBy),
            values: "Id is required");

        invalidStudentException.AddData(
            key: nameof(Student.UpdatedDate),
            "Date is required",
            $"Date is the same as {nameof(Student.CreatedDate)}");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: invalidStudentException);

        // when
        ValueTask<Student> modifyStudentTask =
            _studentService.ModifyStudentAsync(student: invalidStudent);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            modifyStudentTask.AsTask());

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
                actualException.SameExceptionAs(expectedStudentValidationException)));

        _storageBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyWhenStudentConstraintsAreInvalidAndLogItAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        DateTimeOffset dateTime = randomDateTime;
        Student invalidStudent = CreateRandomStudent(date: randomDateTime);
        invalidStudent.UpdatedBy = invalidStudent.CreatedBy;

        invalidStudent.UserId = GetRandomMessage(length: 255);
        invalidStudent.IdentityNumber = GetRandomMessage(length: 255);
        invalidStudent.FirstName = GetRandomMessage(length: 255);
        invalidStudent.MiddleName = GetRandomMessage(length: 255);
        invalidStudent.LastName = GetRandomMessage(length: 255);

        var invalidStudentException =
            new InvalidStudentException(message: "Invalid student. Please fix the errors and try again.");

        invalidStudentException.AddData(
               key: nameof(Student.UserId),
               values: "Text cannot be longer than 100 characters");

        invalidStudentException.AddData(
              key: nameof(Student.IdentityNumber),
              values: "Text cannot be longer than 50 characters");

        invalidStudentException.AddData(
            key: nameof(Student.FirstName),
            values: "Text cannot be longer than 100 characters");

        invalidStudentException.AddData(
            key: nameof(Student.MiddleName),
            values: "Text cannot be longer than 100 characters");

        invalidStudentException.AddData(
           key: nameof(Student.LastName),
           values: "Text cannot be longer than 100 characters");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: invalidStudentException);

        _dateTimeBrokerMock.GetCurrentDateTime()
           .Returns(returnThis: dateTime);

        // when
        ValueTask<Student> modifyStudentTask =
            _studentService.ModifyStudentAsync(invalidStudent);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            modifyStudentTask.AsTask());

        _dateTimeBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .GetCurrentDateTime();

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
                actualException.SameExceptionAs(expectedStudentValidationException)));

        _storageBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }
}
