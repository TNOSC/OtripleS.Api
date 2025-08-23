// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : RESTFulController
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService) =>
        _studentService = studentService;

    [HttpPost]
    public async ValueTask<ActionResult<Student>> PostStudentAsync(Student student)
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

    [HttpPut]
    public async ValueTask<ActionResult<Student>> PutStudentAsync(Student student)
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

    [HttpDelete]
    public async ValueTask<ActionResult<Student>> DeleteStudentAsync(Guid studentId)
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

    [HttpGet("{studentId}")]
    public ValueTask<ActionResult<Student>> GetStudentByIdAsync(Guid studentId) =>
        throw new NotImplementedException();

}
