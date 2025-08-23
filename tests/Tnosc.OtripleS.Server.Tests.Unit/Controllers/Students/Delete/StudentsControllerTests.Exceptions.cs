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
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Controllers.Students;

public partial class StudentsControllerTests
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
            new ActionResult<Student>(expectedBadRequestObjectResult);

        _studentService.RemoveStudentByIdAsync(studentId: someStudentId)
            .ThrowsAsync(ex: validationException);

        // when
        ActionResult<Student> actualActionResult =
            await _studentsController.DeleteStudentAsync(studentId: someStudentId);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(1)
            .RemoveStudentByIdAsync(studentId: someStudentId);
    }
}
