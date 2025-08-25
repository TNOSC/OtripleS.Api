// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Tnosc.Lib.Api;
using Tnosc.OtripleS.Server.Api.Routes;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Home;

public class GetHomeEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<string>
{
    [HttpGet(HomeRoutes.Get, Name = nameof(GetHomeEndpoint))]
    public override ActionResult<string> HandleAsync() =>
        Ok("Hello Mario, the princess is in another castle.");
}
