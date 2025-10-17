// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using NSubstitute;
using Tnosc.OtripleS.Server.Application.Brokers.Storages;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryAccounts;
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;
using Tnosc.OtripleS.Server.Domain.Students;
using Tynamix.ObjectFiller;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.LibraryAccounts;

public partial class LibraryAccountServiceTests
{
    private readonly IStorageBroker _storageBrokerMock;
    private readonly ILibraryAccountService _libraryAccountService;

    public LibraryAccountServiceTests()
    {
        _storageBrokerMock = Substitute.For<IStorageBroker>();

        _libraryAccountService = new LibraryAccountService(
            storageBroker: _storageBrokerMock);
    }

    private static LibraryAccount CreateRandomLibraryAcocunt() =>
        CreateLibraryAccountFiller().Create();

    private static Filler<LibraryAccount> CreateLibraryAccountFiller()
    {
        var filler = new Filler<LibraryAccount>();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(GetRandomDateTime());

        return filler;
    }
    private static DateTimeOffset GetRandomDateTime() =>
           new DateTimeRange(earliestDate: DateTime.UtcNow).GetValue();
}
