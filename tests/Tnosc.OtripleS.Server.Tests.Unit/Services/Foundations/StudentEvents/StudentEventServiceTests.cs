// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using NSubstitute;
using Tnosc.OtripleS.Server.Application.Brokers.Queues;
using Tnosc.OtripleS.Server.Application.Brokers.Queues.Messages;
using Tnosc.OtripleS.Server.Application.Services.Foundations.StudentEvents;
using Tynamix.ObjectFiller;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.StudentEvents;

public partial class StudentEventServiceTests
{
    private readonly IQueueBroker _queueBroker;
    private readonly IStudentEventService _studentEventService;

    public StudentEventServiceTests()
    {
        _queueBroker = Substitute.For<IQueueBroker>();
        _studentEventService = new StudentEventService(queueBroker: _queueBroker);
    }

    private static StudentMessage CreateRandomStudentMessage() =>
        CreateStudentFiller().Create();

    private static Filler<StudentMessage> CreateStudentFiller() =>
        new Filler<StudentMessage>();
}
