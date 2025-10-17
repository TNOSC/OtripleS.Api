// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Tnosc.OtripleS.Server.Application.Brokers.Storages;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal sealed partial class StorageBroker : EFxceptionsContext, IStorageBroker
{
    private readonly IConfiguration _configuration;

    public StorageBroker(IConfiguration configuration) 
        => _configuration = configuration;

    private async ValueTask<T> InsertAsync<T>(T @object)
    {
        using var broker = new StorageBroker(configuration: _configuration);
#pragma warning disable CS8604 // Possible null reference argument.
        broker.Entry(entity: @object).State = EntityState.Added;
#pragma warning restore CS8604 // Possible null reference argument.
        await broker.SaveChangesAsync();
        broker.DetachEntity(@object: @object);

        return @object;
    }

#pragma warning disable CS8603 // Possible null reference return.
    private async ValueTask<T> SelectAsync<T>(params object[] @objectIds)
        where T : class
    {
        using var broker = new StorageBroker(configuration: _configuration);

        return await broker.FindAsync<T>(keyValues: objectIds);
    }
#pragma warning restore CS8603 // Possible null reference return.

#pragma warning disable CA1859 // Use concrete types when possible for improved performance

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    private async ValueTask<IQueryable<T>> SelectAll<T>() where T : class =>
        Set<T>();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

#pragma warning restore CA1859 // Use concrete types when possible for improved performance

    private async ValueTask<T> UpdateAsync<T>(T @object)
    {
        using var broker = new StorageBroker(configuration: _configuration);
#pragma warning disable CS8604 // Possible null reference argument.
        broker.Entry(entity: @object).State = EntityState.Modified;
#pragma warning restore CS8604 // Possible null reference argument.
        await broker.SaveChangesAsync();
        broker.DetachEntity(@object: @object);

        return @object;
    }

    private async ValueTask<T> DeleteAsync<T>(T @object)
    {
        using var broker = new StorageBroker(configuration: _configuration);
#pragma warning disable CS8604 // Possible null reference argument.
        broker.Entry(@object).State = EntityState.Deleted;
#pragma warning restore CS8604 // Possible null reference argument.
        await broker.SaveChangesAsync();
        broker.DetachEntity(@object: @object);

        return @object;
    }

#pragma warning disable CS8604 // Possible null reference argument.
    private void DetachEntity<T>(T @object) => 
        Entry(@object).State = EntityState.Detached;
#pragma warning restore CS8604 // Possible null reference argument.

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(queryTrackingBehavior: QueryTrackingBehavior.NoTracking);
        string connectionString = _configuration.GetConnectionString(name: "DefaultConnection")
            ?? throw new InvalidOperationException(message: "DefaultConnection is not configured");
        optionsBuilder.UseSqlServer(connectionString: connectionString);
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder: modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: typeof(StorageBroker).Assembly);
    }
}
