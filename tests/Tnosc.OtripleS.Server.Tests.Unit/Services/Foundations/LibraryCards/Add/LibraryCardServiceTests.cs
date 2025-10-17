// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.LibraryCards;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.LibraryCards;

public partial class LibraryCardServiceTests
{
    [Fact]
    public async Task ShouldAddLibraryCardAsync()
    {
        // given
        LibraryCard randomLibraryCard =
            CreateRandomLibraryAcocunt();

        LibraryCard inputLibraryCard =
            randomLibraryCard;

        LibraryCard insertedLibraryCard =
            inputLibraryCard;

        LibraryCard expectedLibraryCard =
            insertedLibraryCard.DeepClone();

        _storageBrokerMock
            .InsertLibraryCardAsync(inputLibraryCard)
                .Returns(insertedLibraryCard);

        // when
        LibraryCard actualLibraryCard =
            await _libraryCardService.AddLibraryCardAsync(
                inputLibraryCard);

        // then
        actualLibraryCard.ShouldBeEquivalentTo(expectedLibraryCard);

        await _storageBrokerMock
           .Received(requiredNumberOfCalls: 1)
               .InsertLibraryCardAsync(
                   libraryCard: inputLibraryCard);

        _storageBrokerMock
            .ReceivedCalls()
                .Count()
                    .ShouldBe(expected: 1);
    }
}
