// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Swashbuckle.AspNetCore.Annotations;
using Tnosc.Lib.Api;
using Tnosc.OtripleS.Server.Api.Routes;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Students;

public partial class GetStudentsEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResultValueTask<IQueryable<Student>>
{
    private readonly IStudentService _studentService;

    public GetStudentsEndpoint(IStudentService studentService) =>
        _studentService = studentService;

    [EnableQuery]
    [HttpGet(StudentsRoutes.Get, Name = nameof(GetStudentsEndpoint))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Get all students.",
        Description = "Retrieves all students from the system.",
        Tags = new[] { StudentsRoutes.Tag })]
    public override async ValueTask<ActionResult<IQueryable<Student>>> HandleAsync() =>
        await TryCatch(async () =>
        {
            IQueryable<Student> retrievedAllStudents = await _studentService
                 .RetrieveAllStudentsAsync();

            return Ok(value: retrievedAllStudents);
        },
        withTracing: AddTrace());
}
