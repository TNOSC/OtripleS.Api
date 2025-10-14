// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tnosc.Lib.Application.Configurations;
using Tnosc.OtripleS.Server.Application.Services.Foundations.StudentEvents;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Orchestrations;
using Tnosc.OtripleS.Server.Application.Services.Processings.Students;

namespace Tnosc.OtripleS.Server.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IStudentProcessingService, StudentProcessingService>();
        services.AddScoped<IStudentEventService, StudentEventService>();
        services.AddScoped<IStudentEventOrchestrationService, StudentEventOrchestrationService>();

        services.Configure<RetryConfig>(
            configuration.GetSection("RetryConfig"));

        services.AddSingleton<IRetryConfig>(sp =>
            sp.GetRequiredService<IOptions<RetryConfig>>().Value);
    }
}
