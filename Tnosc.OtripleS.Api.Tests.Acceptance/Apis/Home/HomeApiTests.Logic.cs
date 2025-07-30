// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace Tnosc.OtripleS.Api.Tests.Acceptance.Apis.Home;
public partial class HomeApiTests
{
    [Fact]
    public async Task ShouldReturnHomeMessageAsync()
    {
        // given
        string expectedHomeMessage =
            "Hello Mario, the princess is in another castle.";

        // when
        string actualHomeMessage =
            await _otripleSpiBroker.GetHomeMessageAsync();

        // then
        actualHomeMessage
            .ShouldBeEquivalentTo(expectedHomeMessage);
    }
}

