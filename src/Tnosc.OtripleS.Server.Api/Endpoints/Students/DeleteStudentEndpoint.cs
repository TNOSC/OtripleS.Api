// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tnosc.Lib.Api;
using Tnosc.OtripleS.Server.Api.Routes;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Students;

public class DeleteStudentEndpoint : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResultValueTask<Student>
{
    private readonly IStudentService _studentService;

    public DeleteStudentEndpoint(IStudentService studentService) =>
        _studentService = studentService;

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete(StudentsRoutes.Delete, Name = nameof(DeleteStudentEndpoint))]
    public override async ValueTask<ActionResult<Student>> HandleAsync(
        [FromRoute(Name = StudentsRoutes.ResourceId)] Guid studentId)
    {
        try
        {
            Student deletedStudent =
                await _studentService.RemoveStudentByIdAsync(studentId: studentId);
            return Ok(value: deletedStudent);
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
