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

public sealed partial class StudentProcessingService : IStudentProcessingService
{
    private readonly IStudentService _studentService;
    private readonly ILoggingBroker _loggingBroker;

    public StudentProcessingService(
        IStudentService studentService,
        ILoggingBroker loggingBroker)
    {
        _studentService = studentService;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<Student> UpsertStudentAsync(Student student) =>
    await TryCatch(async () =>
    {
        ValidateStudentOnUpsert(student: student);

        Student mayBeStudent = await _studentService
            .RetrieveStudentByIdAsync(studentId: student.Id);

        return mayBeStudent switch
        {
            null => await _studentService.RegisterStudentAsync(student: student),
            _ => await _studentService.ModifyStudentAsync(student: student)
        };
    });
}

