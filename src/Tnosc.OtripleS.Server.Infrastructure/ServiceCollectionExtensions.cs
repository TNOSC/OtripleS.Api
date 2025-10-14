// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Tnosc.OtripleS.Server.Application.Brokers.DateTimes;
using Tnosc.OtripleS.Server.Application.Brokers.Loggings;
using Tnosc.OtripleS.Server.Application.Brokers.Queues;
using Tnosc.OtripleS.Server.Application.Brokers.Storages;
using Tnosc.OtripleS.Server.Infrastructure.Brokers.DateTimes;
using Tnosc.OtripleS.Server.Infrastructure.Brokers.Jobs;
using Tnosc.OtripleS.Server.Infrastructure.Brokers.Loggings;
using Tnosc.OtripleS.Server.Infrastructure.Brokers.Queues;
using Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

namespace Tnosc.OtripleS.Server.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddBrokers(this IServiceCollection services)
    {
        services.AddDbContext<StorageBroker>();
        services.AddScoped<IStorageBroker, StorageBroker>();
        services.AddSingleton<IQueueBroker, QueueBroker>();
        services.AddTransient<IDateTimeBroker, DateTimeBroker>();
        services.AddTransient<ILoggingBroker, LoggingBroker>();
        services.AddHostedService<DbMigrationHostedService>();
        services.AddHostedService<EventListenerHostedService>();
    }
}
