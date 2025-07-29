// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace Tnosc.OtripleS.Api.Tests.Acceptance.Brokers;

public partial class OtripleSApiBroker
{
    private const string HomeRelativeUrl = "api/home";

    public async ValueTask<string> GetHomeMessageAsync() =>
        await _apiFactoryClient.GetContentStringAsync(HomeRelativeUrl);
}

