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

public class PutStudentEndpoint : EndpointBaseAsync
    .WithRequest<Student>
    .WithActionResultValueTask<Student>
{
    private readonly IStudentService _studentService;

    public PutStudentEndpoint(IStudentService studentService) =>
        _studentService = studentService;

    [HttpPut(StudentsRoutes.Put, Name = nameof(PutStudentEndpoint))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Update a student.",
        Description = "Updates the details of an existing student in the system.",
        Tags = new[] { StudentsRoutes.Tag })]
    public override async ValueTask<ActionResult<Student>> HandleAsync(Student student)
    {
        try
        {
            Student modifiedStudent =
                await _studentService.ModifyStudentAsync(student: student);

            return Ok(value: modifiedStudent);
        }
        catch (StudentValidationException studentValidationException)
            when (studentValidationException.InnerException is NotFoundStudentException)
        {
            return NotFound(exception: studentValidationException.InnerException);
        }
        catch (StudentValidationException studentValidationException)
        {
            return BadRequest(exception: studentValidationException.InnerException);
        }
        catch (StudentDependencyValidationException studentDependencyValidationException)
            when (studentDependencyValidationException.InnerException is LockedStudentException)
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
