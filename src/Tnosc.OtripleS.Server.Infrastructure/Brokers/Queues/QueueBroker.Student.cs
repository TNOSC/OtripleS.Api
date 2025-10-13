// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Tnosc.OtripleS.Server.Application.Brokers.Queues.Messages;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Queues;

public partial class QueueBroker
{
    private Func<StudentMessage, ValueTask>? _studentMessageHandler;
    public ServiceBusProcessor StudentsQueue { get; set; } = null!;

    public async ValueTask ListenToStudentQueueAsync(Func<StudentMessage, ValueTask> messageHandler)
    {
        _studentMessageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));

        StudentsQueue.ProcessMessageAsync += CompleteStudentQueueMessageAsync;
      
        await StudentsQueue.StartProcessingAsync();
    }

    private async Task CompleteStudentQueueMessageAsync(ProcessMessageEventArgs args)
    {
        StudentMessage? message = args.Message.Body.ToObjectFromJson<StudentMessage>();

        if (message is not null && _studentMessageHandler is not null)
        {
            await _studentMessageHandler(message);
            await args.CompleteMessageAsync(args.Message);
        }
    }
}
