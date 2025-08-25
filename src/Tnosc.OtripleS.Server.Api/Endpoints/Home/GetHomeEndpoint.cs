// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tnosc.Lib.Api;
using Tnosc.OtripleS.Server.Api.Routes;

namespace Tnosc.OtripleS.Server.Api.Endpoints.Home;

public class GetHomeEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<string>
{
    [HttpGet(HomeRoutes.Get, Name = nameof(GetHomeEndpoint))]
    [SwaggerOperation(
        Summary = "Test endpoint.",
        Description = "Returns a sample greeting message for testing purposes.",
        Tags = new[] { HomeRoutes.Tag })]
    public override ActionResult<string> Handle() =>
        Ok("Hello Mario, the princess is in another castle.");
}
