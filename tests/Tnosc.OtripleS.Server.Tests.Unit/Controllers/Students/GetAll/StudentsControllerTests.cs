// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Controllers.Students;

public partial class StudentsControllerTests
{
    [Fact]
    public async Task ShouldReturnOkOnGetAsync()
    {
        // given
        IEnumerable<Student> randomStudents = CreateRandomStudents();
        IEnumerable<Student> storageStudents = randomStudents.DeepClone();
        IEnumerable<Student> expectedStudents = storageStudents.DeepClone();

        var expectedObjectResult =
            new OkObjectResult(value: expectedStudents);

        var expectedActionResult =
            new ActionResult<Student>(result: expectedObjectResult);

        _studentService.RetrieveAllStudentsAsync()
            .Returns(returnThis: storageStudents);

        // when
        ActionResult<Student> actualActionResult =
            await _studentsController.GetAllStudentsAsync();

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RetrieveAllStudentsAsync();
    }
}
