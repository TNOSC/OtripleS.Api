// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Enpoints.Students.Put;

public partial class PutStudentEndpointTests
{
    [Fact]
    public async Task ShouldReturnOkOnPutAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        Student inputStudent = randomStudent;
        Student updatedStudent = inputStudent;
        Student expectedStudent = inputStudent.DeepClone();

        var expectedObjectResult =
            new OkObjectResult(value: expectedStudent);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedObjectResult);

        _studentService.ModifyStudentAsync(student: inputStudent)
            .Returns(returnThis: updatedStudent);

        // when
        ActionResult<Student> actualActionResult =
            await _putStudentEndpoint.HandleAsync(student: inputStudent);

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .ModifyStudentAsync(student: inputStudent);
    }
}
