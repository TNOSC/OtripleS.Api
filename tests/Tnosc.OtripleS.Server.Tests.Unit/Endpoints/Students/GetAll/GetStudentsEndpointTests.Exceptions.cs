// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RESTFulSense.Models;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Enpoints.Students.GetAll;

public partial class GetStudentsEndpointTests
{
    [Theory]
    [MemberData(nameof(ServerExceptions))]
    public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(
           Xeption serverException)
    {
        // given
        InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
            InternalServerError(exception: serverException);

        var expectedActionResult =
            new ActionResult<IEnumerable<Student>>(result: expectedInternalServerErrorObjectResult);

        _studentService.RetrieveAllStudentsAsync()
            .ThrowsAsync(ex: serverException);

        // when
        ActionResult<IEnumerable<Student>> actualActionResult =
            await _getStudentsEndpoint.HandleAsync();

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        await _studentService.Received(requiredNumberOfCalls: 1)
            .RetrieveAllStudentsAsync();
    }
}
