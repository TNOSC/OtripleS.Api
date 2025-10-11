// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Tnosc.OtripleS.Server.Application.Observabilities;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

namespace Tnosc.OtripleS.Server.Api.Extensions;

internal static class OpenTelemetryExtensions
{
    private const string HealthEndpointPath = "/health";
    private const string AlivenessEndpointPath = "/alive";
    internal static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddOpenTelemetry()
           .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
           .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation()
                   .AddHttpClientInstrumentation()
                   .AddRuntimeInstrumentation())
           .WithTracing(tracing =>
           {
               tracing.AddSource(DiagnosticsConfig.ServiceName);
               tracing.AddSource(nameof(StudentService));
               tracing.AddAspNetCoreInstrumentation(tracing =>
                    tracing.Filter = context =>
                        !context.Request.Path.StartsWithSegments(HealthEndpointPath, StringComparison.InvariantCulture)
                        && !context.Request.Path.StartsWithSegments(AlivenessEndpointPath, StringComparison.InvariantCulture))
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation(opt => opt.SetDbStatementForText = true);
           });

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        bool useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        return builder;
    }
}
