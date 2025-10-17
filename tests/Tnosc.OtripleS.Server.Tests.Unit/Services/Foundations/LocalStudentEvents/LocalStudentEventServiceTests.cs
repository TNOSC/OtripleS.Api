// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using NSubstitute;
using Tnosc.OtripleS.Server.Application.Brokers.Events;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LocalStudentEvents;
using Tnosc.OtripleS.Server.Domain.LibraryCards;
using Tnosc.OtripleS.Server.Domain.Students;
using Tynamix.ObjectFiller;

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

    private static Student CreateRandomStudent() =>
         CreateStudentFiller().Create();

    private static Filler<Student> CreateStudentFiller()
    {
        var filler = new Filler<Student>();

        filler.Setup()
           .OnType<DateTimeOffset>().Use(GetRandomDateTime());

        return filler;
    }

    private static DateTimeOffset GetRandomDateTime() =>
          new DateTimeRange(earliestDate: DateTime.UtcNow).GetValue();
}
