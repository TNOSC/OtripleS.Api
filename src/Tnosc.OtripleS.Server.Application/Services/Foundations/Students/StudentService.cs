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

public sealed partial class StudentService : IStudentService
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

    public async ValueTask<Student> ModifyStudentAsync(Student student) =>
        await TryCatch(async () =>
        {
        ValidateStudentOnModify(student: student);

            Student maybeStudent =
               await _storageBroker.SelectStudentByIdAsync(studentId: student.Id);

            ValidateStorageStudent(
                storageStudent: maybeStudent,
                studentId: student.Id);

            ValidateAgainstStorageStudentOnModify(inputStudent: student, storageStudent: maybeStudent);

            return await _storageBroker.UpdateStudentAsync(student: student);
        });

    public async ValueTask<Student> RegisterStudentAsync(Student student) =>
        await TryCatch(async () =>
        {
            ValidateStudentOnRegister(student: student);
            return await _storageBroker.InsertStudentAsync(student: student);
        });

    public async ValueTask<Student> RemoveStudentByIdAsync(Guid studentId) 
    {
        Student student =
              await _storageBroker.SelectStudentByIdAsync(studentId: studentId);
        return await _storageBroker.DeleteStudentAsync(student: student);
    }
    
    public ValueTask<IEnumerable<Student>> RetrieveAllStudentsAsync() => 
        throw new NotImplementedException();
    
    public ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) => 
        throw new NotImplementedException();
}
