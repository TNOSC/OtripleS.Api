// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using NSubstitute;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryAccounts;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryCards;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LocalStudentEvents;
using Tnosc.OtripleS.Server.Application.Services.Orchestrations.LibraryAccounts;
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;
using Tnosc.OtripleS.Server.Domain.LibraryCards;
using Tnosc.OtripleS.Server.Domain.Students;
using Tynamix.ObjectFiller;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Orchestrations.LibraryAccounts;

public partial class LibraryAccountOrchestrationServiceTests
{
    private readonly ILibraryAccountService _libraryAccountServiceMock;
    private readonly ILibraryCardService _libraryCardServiceMock;
    private readonly ILocalStudentEventService _localStudentEventServiceMock;
    private readonly ILibraryAccountOrchestrationService _libraryAccountOrchestrationService;

    public LibraryAccountOrchestrationServiceTests()
    {
        _libraryAccountServiceMock = Substitute.For<ILibraryAccountService>();
        _libraryCardServiceMock = Substitute.For<ILibraryCardService>();
        _localStudentEventServiceMock = Substitute.For<ILocalStudentEventService>();

        _libraryAccountOrchestrationService = new LibraryAccountOrchestrationService(
            libraryAccountService: _libraryAccountServiceMock,
            libraryCardService: _libraryCardServiceMock,
            localStudentEventService: _localStudentEventServiceMock);
    }

    private static Expression<Predicate<LibraryCard>> SameLibraryCardAs(
        LibraryCard expectedLibraryCard) => 
            actualLibraryCard =>
                actualLibraryCard.LibraryAccountId == expectedLibraryCard.LibraryAccountId
                && actualLibraryCard.LibraryAccountId != Guid.Empty;

    private static LibraryAccount CreateRandomLibraryAccount() =>
       CreateLibraryAccountFiller().Create();

    private static Filler<LibraryAccount> CreateLibraryAccountFiller()
    {
        var filler = new Filler<LibraryAccount>();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(GetRandomDateTime());

        return filler;
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
