// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tnosc.OtripleS.Server.Api.Swagger;

internal sealed class SwaggerGenOptionsSetup : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.EnableAnnotations();

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "0.0.1",
            Title = "OtripleS API",
            Description = "This swagger document describes the available API endpoints."
        });

        options.CustomSchemaIds(type => type.FullName);
    }
}
