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
    public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync(Xeption validationException)
    {
        // given
        var someStudentId = Guid.NewGuid();

        BadRequestObjectResult expectedBadRequestObjectResult =
            BadRequest(exception: validationException.InnerException);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedBadRequestObjectResult);

        _studentService.RetrieveStudentByIdAsync(studentId: someStudentId)
            .ThrowsAsync(ex: validationException);

        // when
        ActionResult<Student> actualActionResult =
            await _studentsController.GetStudentByIdAsync(studentId: someStudentId);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RetrieveStudentByIdAsync(studentId: someStudentId);
    }
}
