// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Brokers.Loggings;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Processings.Students;

public sealed class StudentProcessingService : IStudentProcessingService
{
#pragma warning disable S4487 // Unread "private" fields should be removed
    private readonly IStudentService _studentService;
    private readonly ILoggingBroker _loggingBroker;
#pragma warning restore S4487 // Unread "private" fields should be removed

    public StudentProcessingService(
        IStudentService studentService, 
        ILoggingBroker loggingBroker)
    {
        _studentService = studentService;
        _loggingBroker = loggingBroker;
    }

    public ValueTask<Student> UpsertStudentAsync(Student student) => 
        throw new System.NotImplementedException();
}

