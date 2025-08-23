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
using RESTFulSense.Models;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Controllers.Students;

public partial class StudentsControllerTests
{
    [Fact]
    public async Task ShouldReturnOkOnDeleteByIdAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        StudentId studentId = randomStudent.Id;
        Student storageSource = randomStudent;
        Student expectedStudent = storageSource.DeepClone();

        var expectedObjectResult =
            new OkObjectResult(expectedStudent);

        var expectedActionResult =
            new ActionResult<Student>(expectedObjectResult);

        _studentService.RemoveStudentByIdAsync(studentId)
            .Returns(returnThis: expectedStudent);

        // when
        ActionResult<Student> actualActionResult =
            await _studentsController.DeleteStudentAsync(studentId);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expectedActionResult);

        await _studentService.Received(1)
            .RemoveStudentByIdAsync(studentId);
    }
}
