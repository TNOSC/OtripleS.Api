// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Enpoints.Students.Delete;

public partial class DeleteStudentEndpointTests
{
    [Fact]
    public async Task ShouldReturnOkOnDeleteByIdAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        Guid studentId = randomStudent.Id;
        Student storageStudent = randomStudent;
        Student expectedStudent = storageStudent.DeepClone();

        var expectedObjectResult =
            new OkObjectResult(value: expectedStudent);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedObjectResult);

        _studentService.RemoveStudentByIdAsync(studentId:studentId)
            .Returns(returnThis: expectedStudent);

        // when
        ActionResult<Student> actualActionResult =
            await _deleteStudentEndpoint.HandleAsync(studentId: studentId);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RemoveStudentByIdAsync(studentId: studentId);
    }
}
