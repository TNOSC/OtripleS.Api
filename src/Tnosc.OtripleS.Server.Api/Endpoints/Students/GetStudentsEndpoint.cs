// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tnosc.Lib.Api;
using Tnosc.OtripleS.Server.Api.Routes;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Students;

public class GetStudentsEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResultValueTask<IEnumerable<Student>>
{
    private readonly IStudentService _studentService;

    public GetStudentsEndpoint(IStudentService studentService) =>
        _studentService = studentService;

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet(StudentsRoutes.Get, Name = nameof(GetStudentsEndpoint))]
    public override async ValueTask<ActionResult<IEnumerable<Student>>> HandleAsync()
    {
        try
        {
            IEnumerable<Student> retrievedAllStudents = await _studentService
                .RetrieveAllStudentsAsync();

            return Ok(value: retrievedAllStudents);
        }
        catch (StudentDependencyException studentDependencyException)
        {
            return InternalServerError(exception: studentDependencyException);
        }
        catch (StudentServiceException studentServiceException)
        {
            return InternalServerError(exception: studentServiceException);
        }
    }
}
