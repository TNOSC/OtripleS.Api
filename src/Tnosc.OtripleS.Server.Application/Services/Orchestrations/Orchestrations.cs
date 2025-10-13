// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Tnosc.OtripleS.Server.Application.Services.Foundations.StudentEvents;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Orchestrations;

public class StudentEventOrchestrationService : IStudentEventOrchestrationService
{
#pragma warning disable S4487 // Unread "private" fields should be removed
    private readonly IStudentEventService _studentEventService;
    private readonly IStudentService _studentService;
#pragma warning restore S4487 // Unread "private" fields should be removed

    public StudentEventOrchestrationService(IStudentEventService studentEventService, IStudentService studentService)
    {
        _studentEventService = studentEventService;
        _studentService = studentService;
    }

    public void ListenToStudentEvents() =>
        throw new System.NotImplementedException();
}
