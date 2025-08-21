// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRegisterWhenStudentIsNullAndLogItAsync()
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
        ValueTask<Student> registerStudentTask =
            _studentService.RegisterStudentAsync(student: invalidStudent);
#pragma warning restore CS8604 // Possible null reference argument.

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            registerStudentTask.AsTask());

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
    public async Task ShouldThrowValidationExceptionOnRegisterWhenStudentIsInvalidAndLogItAsync(string? invalidText)
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
            key: nameof(Student.UpdatedBy),
            values: "Id is required");

        invalidStudentException.AddData(
            key: nameof(Student.CreatedDate),
            values: "Date is required");

        invalidStudentException.AddData(
            key: nameof(Student.UpdatedDate),
            values: "Date is required");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: invalidStudentException);

        // when
        ValueTask<Student> registerStudentTask =
            _studentService.RegisterStudentAsync(student: invalidStudent);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            registerStudentTask.AsTask());

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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnRegisterWhenStudentConstraintsAreInvalidAndLogItAsync()
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
        ValueTask<Student> registerStudentTask =
            _studentService.RegisterStudentAsync(invalidStudent);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            registerStudentTask.AsTask());

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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnRegisterWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
    {
        // given
        DateTimeOffset dateTime = GetRandomDateTime();
        Student randomStudent = CreateRandomStudent(date: dateTime);
        Student invalidStudent = randomStudent;
        invalidStudent.UpdatedBy = Guid.NewGuid();

        var invalidStudentException = 
            new InvalidStudentException(message: "Invalid student. Please fix the errors and try again.");

        invalidStudentException.AddData(
            key: nameof(Student.UpdatedBy),
            values: $"Id is not the same as {nameof(Student.CreatedBy)}");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: invalidStudentException);

        _dateTimeBrokerMock
            .GetCurrentDateTime()
            .Returns(returnThis: dateTime);

        // when
        ValueTask<Student> registerStudentTask =
            _studentService.RegisterStudentAsync(student: invalidStudent);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            registerStudentTask.AsTask());
       
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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnRegisterWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
    {
        // given
        DateTimeOffset dateTime = GetRandomDateTime();
        Student randomStudent = CreateRandomStudent(date: dateTime);
        Student invalidStudent = randomStudent;
        invalidStudent.UpdatedDate = GetRandomDateTime();
        
        var invalidStudentException = 
            new InvalidStudentException(message: "Invalid student. Please fix the errors and try again.");

        invalidStudentException.AddData(
            key: nameof(Student.UpdatedDate),
            values: $"Date is not the same as {nameof(Student.CreatedDate)}");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: invalidStudentException);

        _dateTimeBrokerMock.GetCurrentDateTime()
           .Returns(returnThis: dateTime);

        // when
        ValueTask<Student> registerStudentTask =
            _studentService.RegisterStudentAsync(student: invalidStudent);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            registerStudentTask.AsTask());

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

    [Theory]
#pragma warning disable xUnit1037 // There are fewer theory data type arguments than required by the parameters of the test method
    [MemberData(nameof(InvalidMinuteCases))]
#pragma warning restore xUnit1037 // There are fewer theory data type arguments than required by the parameters of the test method
    public async Task ShouldThrowValidationExceptionOnRegisterWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
    {
        // given
        DateTimeOffset randomDate = GetRandomDateTime();
        Student randomStudent = CreateRandomStudent(date: randomDate);
        Student invalidStudent = randomStudent;
        invalidStudent.CreatedDate = randomDate.AddMinutes(minutes: minutes);
        invalidStudent.UpdatedDate = invalidStudent.CreatedDate;

        var invalidStudentException = 
            new InvalidStudentException(message: "Invalid student. Please fix the errors and try again.");

        invalidStudentException.AddData(
            key: nameof(Student.CreatedDate),
            values: $"Date is not recent");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: invalidStudentException);

        _dateTimeBrokerMock.GetCurrentDateTime()
           .Returns(returnThis: randomDate);

        // when
        ValueTask<Student> registerStudentTask =
            _studentService.RegisterStudentAsync(student: invalidStudent);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            registerStudentTask.AsTask());

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
