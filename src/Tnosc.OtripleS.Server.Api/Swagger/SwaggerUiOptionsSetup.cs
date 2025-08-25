// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Tnosc.OtripleS.Server.Api.Swagger;

internal sealed class SwaggerUiOptionsSetup : IConfigureOptions<SwaggerUIOptions>
{
    public void Configure(SwaggerUIOptions options) => 
        options.DisplayRequestDuration();
}
