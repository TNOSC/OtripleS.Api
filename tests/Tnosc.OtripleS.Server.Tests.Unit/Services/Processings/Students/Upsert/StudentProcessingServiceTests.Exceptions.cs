// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tnosc.OtripleS.Server.Application.Exceptions.Processings.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Processings.Students;

public partial class StudentProcessingServiceTests
{
    [Theory]
    [MemberData(nameof(DependencyValidationExceptions))]
    public async Task ShouldThrowDependencyValidationExceptionOnUpsertIfDependencyValidationErrorOccursAndLogItAsync(
          Xeption dependencyValidationExceptions)
    {
        // given
        Student someStudent = CreateRandomStudent();

        var expectedStudentProcessingDependencyValidationException =
            new StudentProcessingDependencyValidationException(
                message: "Student processing dependency validation error occurred, fix the errors and try again.",
                innerException: (dependencyValidationExceptions.InnerException as Xeption)!);

        _studentServiceMock.RetrieveStudentByIdAsync(studentId: someStudent.Id)
            .ThrowsAsync(ex: dependencyValidationExceptions);

        // when
        ValueTask<Student> upsertStudentTask =
           _studentProcessingService.UpsertStudentAsync(student: someStudent);

        // then
        await Assert.ThrowsAsync<StudentProcessingDependencyValidationException>(() =>
            upsertStudentTask.AsTask());

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
                actualException.SameExceptionAs(expectedStudentProcessingDependencyValidationException)));

        await _studentServiceMock
            .Received(requiredNumberOfCalls: 0)
            .RegisterStudentAsync(student: someStudent);

        await _studentServiceMock
            .Received(requiredNumberOfCalls: 0)
            .ModifyStudentAsync(student: someStudent);
    }

    [Theory]
    [MemberData(nameof(DependencyExceptions))]
    public async Task ShouldThrowDependencyExceptionOnUpsertIfDependencyErrorOccursAndLogItAsync(
          Xeption dependencyValidationExceptions)
    {
        // given
        Student someStudent = CreateRandomStudent();

        var expectedStudentProcessingDependencyValidationException =
            new StudentProcessingDependencyException(
                message: "Student processing dependency error occurred, please contact support.",
                innerException: (dependencyValidationExceptions.InnerException as Xeption)!);

        _studentServiceMock.RetrieveStudentByIdAsync(studentId: someStudent.Id)
            .ThrowsAsync(ex: dependencyValidationExceptions);

        // when
        ValueTask<Student> upsertStudentTask =
           _studentProcessingService.UpsertStudentAsync(student: someStudent);

        // then
        await Assert.ThrowsAsync<StudentProcessingDependencyException>(() =>
            upsertStudentTask.AsTask());

        _loggingBrokerMock.Received(requiredNumberOfCalls: 1)
            .LogError(Arg.Is<Xeption>(actualException =>
                actualException.SameExceptionAs(expectedStudentProcessingDependencyValidationException)));

        await _studentServiceMock
            .Received(requiredNumberOfCalls: 0)
            .RegisterStudentAsync(student: someStudent);

        await _studentServiceMock
            .Received(requiredNumberOfCalls: 0)
            .ModifyStudentAsync(student: someStudent);
    }
}
