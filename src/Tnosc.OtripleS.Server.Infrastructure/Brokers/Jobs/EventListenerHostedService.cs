// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tnosc.OtripleS.Server.Application.Services.Orchestrations.LibraryAccounts;
using Tnosc.OtripleS.Server.Application.Services.Orchestrations.StudentEvents;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Jobs;

internal sealed class EventListenerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventListenerHostedService> _logger;

    public EventListenerHostedService(
        IServiceProvider serviceProvider,
        ILogger<EventListenerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting listening to events...");

        using IServiceScope scope = _serviceProvider.CreateScope();
        IStudentEventOrchestrationService studentEventOrchestrationService =
            scope.ServiceProvider.GetRequiredService<IStudentEventOrchestrationService>();
        ILibraryAccountOrchestrationService libraryAccountOrchestrationService =
            scope.ServiceProvider.GetRequiredService<ILibraryAccountOrchestrationService>();
        try
        {
            await studentEventOrchestrationService.ListenToStudentEventsAsync();
            libraryAccountOrchestrationService.ListenToLocalStudentEvent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Event listener failed.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
