// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Linq;
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
    public void ShouldReturnInternalServerErrorOnGetIfServerErrorOccurred(
           Xeption serverException)
    {
        // given
        InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
            InternalServerError(exception: serverException);

        var expectedActionResult =
            new ActionResult<IQueryable<Student>>(result: expectedInternalServerErrorObjectResult);

        _studentService.RetrieveAllStudents()
            .Throws(ex: serverException);

        // when
        ActionResult<IQueryable<Student>> actualActionResult =
            _getStudentsEndpoint.Handle();

        // then
        actualActionResult.ShouldBeEquivalentTo(
            expected: expectedActionResult);

        _studentService.Received(requiredNumberOfCalls: 1)
           .RetrieveAllStudents();
    }
}
