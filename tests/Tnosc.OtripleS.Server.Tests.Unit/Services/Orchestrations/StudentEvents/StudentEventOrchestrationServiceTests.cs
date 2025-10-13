// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using NSubstitute;
using Tnosc.OtripleS.Server.Application.Brokers.Queues.Messages;
using Tnosc.OtripleS.Server.Application.Services.Foundations.StudentEvents;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Orchestrations;
using Tynamix.ObjectFiller;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Orchestrations.StudentEvents;

public partial class StudentEventOrchestrationServiceTests
{
    private readonly IStudentService _studentServiceMock;
    private readonly IStudentEventService _studentEventServiceMock;
    private readonly IStudentEventOrchestrationService _studentEventOrchestrationService;

    public StudentEventOrchestrationServiceTests()
    {
        _studentServiceMock = Substitute.For<IStudentService>();
        _studentEventServiceMock = Substitute.For<IStudentEventService>();
        _studentEventOrchestrationService = new StudentEventOrchestrationService(
            studentService: _studentServiceMock,
            studentEventService: _studentEventServiceMock);
    }
    private static StudentMessage CreateRandomStudentMessage() =>
       CreateStudentFiller().Create();

    private static Filler<StudentMessage> CreateStudentFiller() =>
        new Filler<StudentMessage>();
}
