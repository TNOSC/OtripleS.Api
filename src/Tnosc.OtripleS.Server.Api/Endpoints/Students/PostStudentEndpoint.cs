// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tnosc.Lib.Api;
using Tnosc.OtripleS.Server.Api.Routes;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Students;

public class PostStudentEndpoint : EndpointBaseAsync
    .WithRequest<Student>
    .WithActionResultValueTask<Student>
{
    private readonly IStudentService _studentService;

    public PostStudentEndpoint(IStudentService studentService) =>
        _studentService = studentService;

    [HttpPost(StudentsRoutes.Post, Name = nameof(PostStudentEndpoint))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Register a new student.",
        Description = "Creates a new student in the system with the provided details.",
        Tags = new[] { StudentsRoutes.Tag })]
    public override async ValueTask<ActionResult<Student>> HandleAsync(Student student)
    {
        try
        {
            Student registeredStudent =
                await _studentService.RegisterStudentAsync(student: student);

            return Created(value: registeredStudent);
        }
        catch (StudentValidationException studentValidationException)
        {
            return BadRequest(exception: studentValidationException.InnerException);
        }
        catch (StudentDependencyValidationException studentDependencyValidationException)
            when (studentDependencyValidationException.InnerException is AlreadyExistsStudentException)
        {
            return Conflict(exception: studentDependencyValidationException.InnerException);
        }
        catch (StudentDependencyValidationException studentDependencyValidationException)
        {
            return BadRequest(exception: studentDependencyValidationException.InnerException);
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
