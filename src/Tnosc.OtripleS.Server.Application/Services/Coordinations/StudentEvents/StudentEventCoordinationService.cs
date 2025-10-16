// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Services.Orchestrations.LibraryAccounts;
using Tnosc.OtripleS.Server.Application.Services.Orchestrations.StudentEvents;

namespace Tnosc.OtripleS.Server.Application.Services.Coordinations.StudentEvents;

public class StudentEventCoordinationService : IStudentEventCoordinationService
{
#pragma warning disable S4487 // Unread "private" fields should be removed
    private readonly IStudentEventOrchestrationService _studentEventOrchestrationService;
    private readonly ILibraryAccountOrchestrationService _libraryAccountOrchestrationService;
#pragma warning restore S4487 // Unread "private" fields should be removed

    public StudentEventCoordinationService(
        IStudentEventOrchestrationService studentEventOrchestrationService,
        ILibraryAccountOrchestrationService libraryAccountOrchestrationService)
    {
        _studentEventOrchestrationService = studentEventOrchestrationService;
        _libraryAccountOrchestrationService = libraryAccountOrchestrationService;
    }
   

    public Task ListenToStudentEventsAsync() =>
        throw new System.NotImplementedException();
}
