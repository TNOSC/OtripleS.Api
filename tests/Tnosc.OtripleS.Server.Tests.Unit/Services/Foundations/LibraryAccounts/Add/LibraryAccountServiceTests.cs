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
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.LibraryAccounts;

public partial class LibraryAccountServiceTests
{
    [Fact]
    public async Task ShouldAddLibraryAccountAsync()
    {
        // given
        LibraryAccount randomLibraryAccount =
            CreateRandomLibraryAcocunt();

        LibraryAccount inputLibraryAccount =
            randomLibraryAccount;

        LibraryAccount insertedLibraryAccount =
            inputLibraryAccount;

        LibraryAccount expectedLibraryAccount =
            insertedLibraryAccount.DeepClone();

        _storageBrokerMock
            .InsertLibraryAccountAsync(
                libraryAccount: inputLibraryAccount)
                    .Returns(insertedLibraryAccount);

        // when
        LibraryAccount actualLibraryAccount =
            await _libraryAccountService.AddLibraryAccountAsync(
                libraryAccount: inputLibraryAccount);

        // then
        actualLibraryAccount.
            ShouldBeEquivalentTo(expected: expectedLibraryAccount);

        await _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
                .InsertLibraryAccountAsync(
                    libraryAccount: inputLibraryAccount);

        _storageBrokerMock
            .ReceivedCalls()
                .Count()
                    .ShouldBe(expected: 1);
    }
}
