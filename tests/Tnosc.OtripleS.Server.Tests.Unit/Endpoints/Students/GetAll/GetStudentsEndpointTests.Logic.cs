// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Enpoints.Students.GetAll;

public partial class GetStudentsEndpointTests
{
    [Fact]
    public async Task ShouldReturnOkOnGet()
    {
        // given
        IQueryable<Student> randomStudents = CreateRandomStudents();
        IQueryable<Student> storageStudents = randomStudents.DeepClone();
        IQueryable<Student> expectedStudents = storageStudents.DeepClone();

        var expectedObjectResult =
            new OkObjectResult(value: expectedStudents);

        var expectedActionResult =
            new ActionResult<IQueryable<Student>>(result: expectedObjectResult);

        _studentService.RetrieveAllStudentsAsync()
            .Returns(returnThis: storageStudents);

        // when
        ActionResult<IQueryable<Student>> actualActionResult =
            await _getStudentsEndpoint.HandleAsync();

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RetrieveAllStudentsAsync();
    }
}
