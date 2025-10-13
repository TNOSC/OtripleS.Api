// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Tnosc.OtripleS.Server.Application.Brokers.Queues;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Queues;

public partial class QueueBroker : IQueueBroker, IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ServiceBusClient _client;
    
    public QueueBroker(IConfiguration configuration)
    {
        _configuration = configuration;
        string connectionString =
            _configuration.GetConnectionString("ServiceBusConnection")
                ?? throw new InvalidOperationException("ServiceBusConnection is not configured");

        _client = new ServiceBusClient(connectionString);
        InitializeQueueProcessor();
    }

    private void InitializeQueueProcessor() =>
        StudentsQueue = GetQueueProcessor(nameof(StudentsQueue));

    private ServiceBusProcessor GetQueueProcessor(string queueName)
    {
        ServiceBusProcessor processor = _client.CreateProcessor(queueName, GetProcessorOptions());
        processor.ProcessErrorAsync += ErrorHandlerAsync;
        return processor;
    }

    private static ServiceBusProcessorOptions GetProcessorOptions() =>
        new ServiceBusProcessorOptions()
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1
        };

    private static Task ErrorHandlerAsync(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Error: {args.Exception}");
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _client.DisposeAsync();
    }
}   
