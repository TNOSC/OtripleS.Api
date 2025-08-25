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

namespace Tnosc.OtripleS.Server.Tests.Unit.Enpoints.Students.Post;

public partial class PostStudentEndpointTests
{
    [Theory]
    [MemberData(nameof(ValidationExceptions))]
    public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync(Xeption validationException)
    {
        // given
        Student someStudent = CreateRandomStudent();

        BadRequestObjectResult expectedBadRequestObjectResult =
            BadRequest(exception: validationException.InnerException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedBadRequestObjectResult);

        _studentService.RegisterStudentAsync(student: someStudent)
            .ThrowsAsync(ex: validationException);

        // when
        ActionResult<Student> actualActionResult =
            await _postStudentEndpoint.HandleAsync(student: someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RegisterStudentAsync(student: someStudent);
    }

    [Theory]
    [MemberData(nameof(ServerExceptions))]
    public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption serverException)
    {
        // given
        Student someStudent = CreateRandomStudent();

        InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
            InternalServerError(exception: serverException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedInternalServerErrorObjectResult);

        _studentService.RegisterStudentAsync(student: someStudent)
            .ThrowsAsync(ex: serverException); 

        // when
        ActionResult<Student> actualActionResult =
            await _postStudentEndpoint.HandleAsync(student: someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RegisterStudentAsync(student: someStudent);
    }

    [Fact]
    public async Task ShouldReturnConflictOnPostIfAlreadyExistsStudentErrorOccurredAsync()
    {
        // given
        Student someStudent = CreateRandomStudent();
        var someInnerException = new Exception();
        string someMessage = GetRandomString();

        var alreadyExistsStudentException =
            new AlreadyExistsStudentException(
                message: someMessage,
                innerException: someInnerException);

        var studentDependencyValidationException =
            new StudentDependencyValidationException(
                message: someMessage,
                innerException: alreadyExistsStudentException);

        ConflictObjectResult expectedConflictObjectResult =
            Conflict(exception: alreadyExistsStudentException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedConflictObjectResult);

        _studentService.RegisterStudentAsync(student: someStudent)
           .ThrowsAsync(ex: studentDependencyValidationException);

        // when
        ActionResult<Student> actualActionResult =
            await _postStudentEndpoint.HandleAsync(student: someStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RegisterStudentAsync(student: someStudent);
    }
}
