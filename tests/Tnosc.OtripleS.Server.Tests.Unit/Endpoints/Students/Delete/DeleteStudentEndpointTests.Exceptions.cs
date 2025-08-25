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

namespace Tnosc.OtripleS.Server.Tests.Unit.Enpoints.Students.Delete;

public partial class DeleteStudentEndpointTests
{
    [Theory]
    [MemberData(nameof(ValidationExceptions))]
    public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
    {
        // given
        var someStudentId = Guid.NewGuid();

        BadRequestObjectResult expectedBadRequestObjectResult =
            BadRequest(exception: validationException.InnerException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedBadRequestObjectResult);

        _studentService.RemoveStudentByIdAsync(studentId: someStudentId)
            .ThrowsAsync(ex: validationException);

        // when
        ActionResult<Student> actualActionResult =
             await _deleteStudentEndpoint.HandleAsync(studentId: someStudentId);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RemoveStudentByIdAsync(studentId: someStudentId);
    }

    [Theory]
    [MemberData(nameof(ServerExceptions))]
    public async Task ShouldReturnInternalServerErrorOnDeleteIfServerErrorOccurredAsync(
           Xeption serverException)
    {
        // given
        var someStudentId = Guid.NewGuid();

        InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
            InternalServerError(exception: serverException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedInternalServerErrorObjectResult);

        _studentService.RemoveStudentByIdAsync(studentId: someStudentId)
            .ThrowsAsync(ex: serverException);

        // when
        ActionResult<Student> actualActionResult =
            await _deleteStudentEndpoint.HandleAsync(studentId: someStudentId);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RemoveStudentByIdAsync(studentId: someStudentId);
    }

    [Fact]
    public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
    {
        // given
        var someStudentId = Guid.NewGuid();
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

        _studentService.RemoveStudentByIdAsync(studentId: someStudentId)
            .ThrowsAsync(ex: studentValidationException);

        // when
        ActionResult<Student> actualActionResult =
            await _deleteStudentEndpoint.HandleAsync(studentId: someStudentId);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RemoveStudentByIdAsync(studentId: someStudentId);
    }

    [Fact]
    public async Task ShouldReturnConflictOnDeleteIfLockedStudentErrorOccurredAsync()
    {
        // given
        var someStudentId = Guid.NewGuid();
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

        _studentService.RemoveStudentByIdAsync(studentId: someStudentId)
            .ThrowsAsync(ex: studentDependencyValidationException);

        // when
        ActionResult<Student> actualActionResult =
            await _deleteStudentEndpoint.HandleAsync(studentId: someStudentId);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RemoveStudentByIdAsync(studentId: someStudentId);
    }
}
