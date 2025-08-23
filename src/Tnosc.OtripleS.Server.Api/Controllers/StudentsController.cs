// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

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
                await _studentService.RegisterStudentAsync(student);

            return Created(registeredStudent);
        }
        catch (StudentValidationException studentValidationException)
        {
            return BadRequest(studentValidationException.InnerException);
        }
        catch (StudentDependencyValidationException studentDependencyValidationException)
        {
            return BadRequest(studentDependencyValidationException.InnerException);
        }
        catch (StudentDependencyException studentDependencyException)
        {
            return InternalServerError(studentDependencyException);
        }
        catch (StudentServiceException studentServiceException)
        {
            return InternalServerError(studentServiceException);
        }
    }
}
