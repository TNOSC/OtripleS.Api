// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Tnosc.OtripleS.Server.Tests.Acceptance.Brokers;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Acceptance.Apis.Home;

[Collection(nameof(ApiTestCollection))]
public partial class HomeApiTests
{
    private readonly OtripleSApiBroker _otripleSpiBroker;

    public HomeApiTests(OtripleSApiBroker sofeeApiBroker) =>
        _otripleSpiBroker = sofeeApiBroker;
}
