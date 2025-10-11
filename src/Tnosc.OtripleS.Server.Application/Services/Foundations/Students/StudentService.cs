// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Tnosc.Lib.Application.Configurations;
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
    private readonly IRetryConfig _retryConfig;

    public StudentService(
        IStorageBroker storageBroker,
        IDateTimeBroker dateTimeBroker,
        ILoggingBroker loggingBroker,
        IRetryConfig retryConfig)
    {
        _storageBroker = storageBroker;
        _dateTimeBroker = dateTimeBroker;
        _loggingBroker = loggingBroker;
        _retryConfig = retryConfig;
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
    },
    withTracing: AddTraceOnModify(student),
    withRetryOn: GetRetryableExceptions());

    public async ValueTask<Student> RegisterStudentAsync(Student student) =>
    await TryCatch(async () =>
    {
        ValidateStudentOnRegister(student: student);
        return await _storageBroker.InsertStudentAsync(student: student);
    },
    withTracing: AddTraceOnRegister(student),
    withRetryOn: GetRetryableExceptions());

    public async ValueTask<Student> RemoveStudentByIdAsync(Guid studentId) =>
    await TryCatch(async () =>
    {
        ValidateStudentId(studentId);

        Student maybeStudent =
            await _storageBroker.SelectStudentByIdAsync(studentId: studentId);

        ValidateStorageStudent(maybeStudent, studentId);

        return await _storageBroker.DeleteStudentAsync(student: maybeStudent);
    });

    public IQueryable<Student> RetrieveAllStudents() =>
    TryCatch(_storageBroker.SelectAllStudents);

    public async ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) =>
    await TryCatch(async () =>
    {
        ValidateStudentId(studentId);

        Student maybeStudent =
            await _storageBroker.SelectStudentByIdAsync(studentId: studentId);

        ValidateStorageStudent(maybeStudent, studentId);

        return maybeStudent;
    });
}
