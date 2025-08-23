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

namespace Tnosc.OtripleS.Server.Tests.Unit.Controllers.Students;

public partial class StudentsControllerTests
{
    [Theory]
    [MemberData(nameof(ValidationExceptions))]
    public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync(Xeption validationException)
    {
        // given
        Student someStudent = CreateRandomStudent();

        BadRequestObjectResult expectedBadRequestObjectResult =
            BadRequest(validationException.InnerException);

        var expectedActionResult =
            new ActionResult<Student>(expectedBadRequestObjectResult);

        _studentService.ModifyStudentAsync(someStudent)
            .ThrowsAsync(validationException);

        // when
        ActionResult<Student> actualActionResult =
            await _studentsController.PutStudentAsync(someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expectedActionResult);

        await _studentService.Received(1)
            .ModifyStudentAsync(someStudent);
    }

    [Theory]
    [MemberData(nameof(ServerExceptions))]
    public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
           Xeption serverException)
    {
        // given
        Student someStudent = CreateRandomStudent();

        InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
            InternalServerError(serverException);

        var expectedActionResult =
            new ActionResult<Student>(expectedInternalServerErrorObjectResult);

        _studentService.ModifyStudentAsync(someStudent)
            .ThrowsAsync(serverException);

        // when
        ActionResult<Student> actualActionResult =
            await _studentsController.PutStudentAsync(someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expectedActionResult);

        await _studentService.Received(1)
            .ModifyStudentAsync(someStudent);
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
            NotFound(notFoundStudentException);

        var expectedActionResult =
            new ActionResult<Student>(expectedNotFoundObjectResult);

        _studentService.ModifyStudentAsync(someStudent)
            .ThrowsAsync(studentValidationException);

        // when
        ActionResult<Student> actualActionResult =
            await _studentsController.PutStudentAsync(someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expectedActionResult);

        await _studentService.Received(1)
            .ModifyStudentAsync(someStudent);
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
            Conflict(lockedStudentException);

        var expectedActionResult =
            new ActionResult<Student>(expectedConflictObjectResult);

        _studentService.ModifyStudentAsync(someStudent)
            .ThrowsAsync(studentDependencyValidationException);

        // when
        ActionResult<Student> actualActionResult =
            await _studentsController.PutStudentAsync(someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expectedActionResult);

        await _studentService.Received(1)
            .ModifyStudentAsync(someStudent);
    }
}
