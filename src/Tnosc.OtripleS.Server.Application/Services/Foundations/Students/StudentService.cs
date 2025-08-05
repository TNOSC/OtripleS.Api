// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Brokers.DateTimes;
using Tnosc.OtripleS.Server.Application.Brokers.Loggings;
using Tnosc.OtripleS.Server.Application.Brokers.Storages;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

public sealed class StudentService : IStudentService
{
    private readonly IStorageBroker _storageBroker;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILoggingBroker _loggingBroker;

    public StudentService(
        IStorageBroker storageBroker,
        IDateTimeBroker dateTimeBroker,
        ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _dateTimeBroker = dateTimeBroker;
        _loggingBroker = loggingBroker;
    }
    public ValueTask<Student> ModifyStudentAsync(Student student) => 
        throw new NotImplementedException();
    
    public ValueTask<Student> RegisterStudentAsync(Student student) => 
        throw new NotImplementedException();
    
    public ValueTask<Student> RemoveStudentByIdAsync(Guid studentId) => 
        throw new NotImplementedException();
    
    public ValueTask<IEnumerable<Student>> RetrieveAllStudentsAsync() => 
        throw new NotImplementedException();
    
    public ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) => 
        throw new NotImplementedException();
}
