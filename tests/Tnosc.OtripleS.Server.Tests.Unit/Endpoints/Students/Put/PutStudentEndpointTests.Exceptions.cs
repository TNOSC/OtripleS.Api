// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RESTFulSense.Models;
using Shouldly;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Enpoints.Students.Put;

public partial class PutStudentEndpointTests
{
    [Theory]
    [MemberData(nameof(ValidationExceptions))]
    public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync(Xeption validationException)
    {
        // given
        Student someStudent = CreateRandomStudent();

        BadRequestObjectResult expectedBadRequestObjectResult =
            BadRequest(exception: validationException.InnerException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedBadRequestObjectResult);

        _studentService.ModifyStudentAsync(student: someStudent)
            .ThrowsAsync(ex: validationException);

        // when
        ActionResult<Student> actualActionResult =
            await _putStudentEndpoint.HandleAsync(student: someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .ModifyStudentAsync(student: someStudent);
    }

    [Theory]
    [MemberData(nameof(ServerExceptions))]
    public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
           Xeption serverException)
    {
        // given
        Student someStudent = CreateRandomStudent();

        InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
            InternalServerError(exception: serverException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedInternalServerErrorObjectResult);

        _studentService.ModifyStudentAsync(student: someStudent)
            .ThrowsAsync(ex: serverException);

        // when
        ActionResult<Student> actualActionResult =
            await _putStudentEndpoint.HandleAsync(student: someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .ModifyStudentAsync(student: someStudent);
    }

    [Fact]
    public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
    {
        // given
        Student someStudent = CreateRandomStudent();
        string someMessage = GetRandomString();

        var notFoundStudentException =
            new NotFoundStudentException(
                message: someMessage);

        var studentValidationException =
            new StudentValidationException(
                message: someMessage,
                innerException: notFoundStudentException);

        NotFoundObjectResult expectedNotFoundObjectResult =
            NotFound(exception: notFoundStudentException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedNotFoundObjectResult);

        _studentService.ModifyStudentAsync(student: someStudent)
            .ThrowsAsync(ex: studentValidationException);

        // when
        ActionResult<Student> actualActionResult =
            await _putStudentEndpoint.HandleAsync(student: someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .ModifyStudentAsync(student: someStudent);
    }

    [Fact]
    public async Task ShouldReturnConflictOnPutIfLockedStudentErrorOccurredAsync()
    {
        // given
        Student someStudent = CreateRandomStudent();
        var someInnerException = new Exception();
        string someMessage = GetRandomString();

        var lockedStudentException =
            new LockedStudentException(
                message: someMessage,
                innerException: someInnerException);

        var studentDependencyValidationException =
            new StudentDependencyValidationException(
                message: someMessage,
                innerException: lockedStudentException);

        ConflictObjectResult expectedConflictObjectResult =
            Conflict(exception: lockedStudentException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedConflictObjectResult);

        _studentService.ModifyStudentAsync(student: someStudent)
            .ThrowsAsync(ex: studentDependencyValidationException);

        // when
        ActionResult<Student> actualActionResult =
            await _putStudentEndpoint.HandleAsync(student: someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .ModifyStudentAsync(student: someStudent);
    }
}
