// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Tnosc.OtripleS.Server.Api.Swagger;

namespace Tnosc.OtripleS.Server.Api;

internal static class ServiceCollectionExtensions
{
    internal static void AddEndpoints(this IServiceCollection services)
    {
        services.AddControllers();
        services.ConfigureOptions<SwaggerGenOptionsSetup>();
        services.ConfigureOptions<SwaggerUiOptionsSetup>();
        services.AddSwaggerGen();
    }
}
