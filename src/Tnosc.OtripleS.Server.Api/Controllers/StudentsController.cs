// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
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
    public ValueTask<ActionResult<Student>> PostStudentAsync(Student student) =>
        throw new NotImplementedException();
}
