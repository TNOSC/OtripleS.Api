// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using NSubstitute;
using Tnosc.OtripleS.Server.Application.Brokers.Events;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LocalStudentEvents;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.LocalStudentEvents;

public partial class LocalStudentEventServiceTests
{
    private readonly IEventBroker _eventBrokerMock;
    private readonly ILocalStudentEventService _localStudentEventService;

    public LocalStudentEventServiceTests()
    {
        _eventBrokerMock = Substitute.For<IEventBroker>();

        _localStudentEventService = new LocalStudentEventService(
            eventBroker: _eventBrokerMock);
    }
}
