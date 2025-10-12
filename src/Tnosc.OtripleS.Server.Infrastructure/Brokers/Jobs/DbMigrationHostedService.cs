// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Jobs;

internal sealed class DbMigrationHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DbMigrationHostedService> _logger;

    public DbMigrationHostedService(
        IServiceProvider serviceProvider,
        ILogger<DbMigrationHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting database migration...");

        using IServiceScope scope = _serviceProvider.CreateScope();
        StorageBroker dbContext = scope.ServiceProvider.GetRequiredService<StorageBroker>();

        try
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database migration failed.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => 
        Task.CompletedTask;
}
