// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using RESTFulSense.Clients;

namespace Tnosc.OtripleS.Server.Tests.Acceptance.Brokers;

public partial class OtripleSApiBroker
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _httpClient;
    private readonly IRESTFulApiFactoryClient _apiFactoryClient;

    public OtripleSApiBroker()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _httpClient = _webApplicationFactory.CreateClient();
        _apiFactoryClient = new RESTFulApiFactoryClient(httpClient: _httpClient);
    }
}
