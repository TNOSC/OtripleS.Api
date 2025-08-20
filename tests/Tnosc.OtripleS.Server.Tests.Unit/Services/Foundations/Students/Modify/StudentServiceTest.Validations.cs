// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
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
        Student invalidStudent = CreateRandomStudent();
        invalidStudent.UpdatedDate = randomDateTime;

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
            .Returns(returnThis: randomDateTime);

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


    [Theory]
#pragma warning disable xUnit1037 // There are fewer theory data type arguments than required by the parameters of the test method
    [MemberData(nameof(InvalidMinuteCases))]
#pragma warning restore xUnit1037 // There are fewer theory data type arguments than required by the parameters of the test method
    public async Task ShouldThrowValidationExceptionOnModifyIfStudentUpdatedDateIsNotRecentAndLogItAsync(
           int randomMoreOrLessThanOneMinute)
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        Student randomStudent = CreateRandomStudent(date: randomDateTime);
        Student invalidStudent = randomStudent;
        invalidStudent.UpdatedDate = randomDateTime.AddMinutes(minutes: randomMoreOrLessThanOneMinute);

        var invalidStudentException =
            new InvalidStudentException(message: "Invalid student. Please fix the errors and try again.");

        invalidStudentException.AddData(
            key: nameof(Student.UpdatedDate),
            values: $"Date is not recent");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: invalidStudentException);

        _dateTimeBrokerMock.GetCurrentDateTime()
            .Returns(returnThis: randomDateTime);

        // when
        ValueTask<Student> modifyStudentTask =
            _studentService.ModifyStudentAsync(student: invalidStudent);

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

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfStudentDoesNotExistAndLogItAsync()
    {
        // given
        DateTimeOffset randomDateTime = GetRandomDateTime();
        Student randomStudent = CreateRandomStudent();
        Student nonExistentStudent = randomStudent;
        nonExistentStudent.UpdatedDate = randomDateTime;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Student noStudent = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        var notFoundStudentException = new NotFoundStudentException(message: $"Couldn't find student with id: {nonExistentStudent.Id.Value}.");

        var expectedStudentValidationException =
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: notFoundStudentException);

        _dateTimeBrokerMock.GetCurrentDateTime()
            .Returns(returnThis: randomDateTime);

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _storageBrokerMock.SelectStudentByIdAsync(studentId: nonExistentStudent.Id)
          .Returns(returnThis: noStudent);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // when
        ValueTask<Student> modifyStudentTask =
            _studentService.ModifyStudentAsync(student: nonExistentStudent);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            modifyStudentTask.AsTask());

        _dateTimeBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .GetCurrentDateTime();

        await _storageBrokerMock
           .Received(requiredNumberOfCalls: 1)
           .SelectStudentByIdAsync(nonExistentStudent.Id);

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentValidationException)));
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
    {
        // given
        int randomNumber = GetRandomNumber();
        DateTimeOffset randomDateTime = GetRandomDateTime();
        var differentId = Guid.NewGuid();
        Guid invalidCreatedBy = differentId;
        Student randomStudent = CreateRandomStudent(date: randomDateTime);
        Student invalidStudent = randomStudent;
        Student storageStudent = randomStudent.DeepClone();
        invalidStudent.CreatedDate = storageStudent.CreatedDate.AddDays(randomNumber);
        invalidStudent.UpdatedDate = randomDateTime;
        invalidStudent.CreatedBy = invalidCreatedBy;
        StudentId studentId = invalidStudent.Id;

        var invalidStudentException =
            new InvalidStudentException(message: "Invalid student. Please fix the errors and try again.");

        invalidStudentException.AddData(
            key: nameof(Student.CreatedDate),
            values: $"Date is not the same as {nameof(Student.CreatedDate)}");
        invalidStudentException.AddData(
            key: nameof(Student.CreatedBy),
            values: $"Id is not the same as {nameof(Student.CreatedBy)}");

        var expectedStudentValidationException =
          new StudentValidationException(
              message: "Invalid input, fix the errors and try again.",
              innerException: invalidStudentException);

        _dateTimeBrokerMock.GetCurrentDateTime()
           .Returns(returnThis: randomDateTime);

        _storageBrokerMock.SelectStudentByIdAsync(studentId: studentId)
          .Returns(returnThis: storageStudent);

        // when
        ValueTask<Student> modifyStudentTask =
            _studentService.ModifyStudentAsync(student: invalidStudent);

        // then
        await Assert.ThrowsAsync<StudentValidationException>(() =>
            modifyStudentTask.AsTask());

        _dateTimeBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .GetCurrentDateTime();

        await _storageBrokerMock
           .Received(requiredNumberOfCalls: 1)
           .SelectStudentByIdAsync(invalidStudent.Id);

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
              actualException.SameExceptionAs(expectedStudentValidationException)));
    }
}
