// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using NSubstitute;
using Tnosc.OtripleS.Server.Application.Brokers.Storages;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryCards;
using Tnosc.OtripleS.Server.Domain.LibraryCards;
using Tynamix.ObjectFiller;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.LibraryCards;

public partial class LibraryCardServiceTests
{
    private readonly IStorageBroker _storageBrokerMock;
    private readonly ILibraryCardService _libraryCardService;

    public LibraryCardServiceTests()
    {
        _storageBrokerMock = Substitute.For<IStorageBroker>();

        _libraryCardService = new LibraryCardService(
             storageBroker: _storageBrokerMock);
    }

    private static LibraryCard CreateRandomLibraryAcocunt() =>
        CreateLibraryCardFiller().Create();

    private static Filler<LibraryCard> CreateLibraryCardFiller()
    {
        var filler = new Filler<LibraryCard>();

        filler.Setup()
            .OnType<DateTimeOffset>().Use(GetRandomDateTime());

        return filler;
    }
    private static DateTimeOffset GetRandomDateTime() =>
           new DateTimeRange(earliestDate: DateTime.UtcNow).GetValue();
}
