// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

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
    public async Task ShouldReturnCreatedOnPostAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        Student inputStudent = randomStudent;
        Student addedStudent = inputStudent;
        Student expectedStudent = inputStudent.DeepClone();

        var expectedObjectResult =
            new CreatedObjectResult(value: expectedStudent);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedObjectResult);

        _studentService.RegisterStudentAsync(student: inputStudent)
            .Returns(returnThis: addedStudent);

        // when
        ActionResult<Student> actualActionResult =
            await _studentsController.PostStudentAsync(student: inputStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(1)
            .RegisterStudentAsync(student: inputStudent);
    }
}
