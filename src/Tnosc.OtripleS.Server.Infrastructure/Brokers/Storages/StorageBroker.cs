// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tnosc.OtripleS.Server.Application.Brokers.Storages;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal sealed partial class StorageBroker : EFxceptionsContext, IStorageBroker
{
    private readonly IConfiguration _configuration;

    public StorageBroker(IConfiguration configuration)
    {
        _configuration = configuration;
        Database.Migrate();
    }

    private async ValueTask<T> InsertAsync<T>(T @object)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        Entry(@object).State = EntityState.Added;
#pragma warning restore CS8604 // Possible null reference argument.
        await SaveChangesAsync();

        return @object;
    }

#pragma warning disable CS8603 // Possible null reference return.
    private async ValueTask<T> SelectAsync<T>(params object[] @objectIds)
        where T : class =>
        await FindAsync<T>(objectIds);
#pragma warning restore CS8603 // Possible null reference return.

    private async ValueTask<IEnumerable<T>> SelectAllAsync<T>() where T : class =>
        await Set<T>().ToListAsync();

    private async ValueTask<T> UpdateAsync<T>(T @object)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        Entry(@object).State = EntityState.Modified;
#pragma warning restore CS8604 // Possible null reference argument.
        await SaveChangesAsync();

        return @object;
    }

    private async ValueTask<T> DeleteAsync<T>(T @object)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        Entry(@object).State = EntityState.Deleted;
#pragma warning restore CS8604 // Possible null reference argument.
        await SaveChangesAsync();

        return @object;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(queryTrackingBehavior: QueryTrackingBehavior.NoTracking);
        string connectionString = _configuration.GetConnectionString(name: "DefaultConnection")
            ?? throw new InvalidOperationException(message: "DefaultConnection is not configured");
        optionsBuilder.UseSqlServer(connectionString: connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder: modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: typeof(StorageBroker).Assembly);
    }
}
