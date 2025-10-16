// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;
using Tnosc.OtripleS.Server.Domain.LibraryCards;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Orchestrations.LibraryAccounts;

public partial class LibraryAccountOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldCreateLibraryAccountAsync()
    {
        // given
        LibraryAccount randomLibraryAccount =
            CreateRandomLibraryAccount();

        LibraryAccount inputLibraryAccount =
            randomLibraryAccount;

        LibraryAccount addedLibraryAccount =
            inputLibraryAccount;

        LibraryAccount expectedLibraryAccount =
            addedLibraryAccount.DeepClone();

        var expectedInputLibraryCard = new LibraryCard
        {
            Id = Guid.NewGuid(),
            LibraryAccountId = addedLibraryAccount.Id
        };

        _libraryAccountServiceMock
            .AddLibraryAccountAsync(libraryAccount: inputLibraryAccount)
                .Returns(addedLibraryAccount);

        // when
        LibraryAccount actualLibraryAccount =
            await _libraryAccountOrchestrationService
                .CreateLibraryAccountAsync(
                    libraryAccount: inputLibraryAccount);

        // then
        actualLibraryAccount.ShouldBeEquivalentTo(expectedLibraryAccount);

        await _libraryAccountServiceMock
          .Received(requiredNumberOfCalls: 1)
              .AddLibraryAccountAsync(
                  libraryAccount: inputLibraryAccount);

        await _libraryCardServiceMock
          .Received(requiredNumberOfCalls: 1)
            .AddLibraryCardAsync(Arg.Is(SameLibraryCardAs(expectedInputLibraryCard)));

        Received.InOrder(async() =>
        {
            await _libraryAccountServiceMock .AddLibraryAccountAsync(
                  libraryAccount: inputLibraryAccount);

            await _libraryCardServiceMock.AddLibraryCardAsync(
                 Arg.Any<LibraryCard>());
        });

        _libraryAccountServiceMock
           .ReceivedCalls()
               .Count()
                   .ShouldBe(expected: 1);

        _libraryCardServiceMock
           .ReceivedCalls()
               .Count()
                   .ShouldBe(expected: 1);
    }
}
