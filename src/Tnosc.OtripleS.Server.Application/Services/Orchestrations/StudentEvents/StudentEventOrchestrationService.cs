// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Brokers.DateTimes;
using Tnosc.OtripleS.Server.Application.Brokers.Queues.Messages;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LocalStudentEvents;
using Tnosc.OtripleS.Server.Application.Services.Foundations.StudentEvents;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Orchestrations.StudentEvents;

public class StudentEventOrchestrationService : IStudentEventOrchestrationService
{
    private readonly IStudentEventService _studentEventService;
    private readonly IStudentService _studentService;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILocalStudentEventService _localStudentEventService;

    public StudentEventOrchestrationService(
        IStudentEventService studentEventService,
        IStudentService studentService,
        IDateTimeBroker dateTimeBroker,
        ILocalStudentEventService localStudentEventService)
    {
        _studentEventService = studentEventService;
        _studentService = studentService;
        _dateTimeBroker = dateTimeBroker;
        _localStudentEventService = localStudentEventService;
    }

    public async Task ListenToStudentEventsAsync() =>
        await _studentEventService.ListenToStudentEventAsync(async studentMessage =>
        {
            Student student = MapToStudent(studentMessage: studentMessage);
            await _studentService.RegisterStudentAsync(student: student);
            await _localStudentEventService.PublishStudentAsync(student: student);
        });

    private Student MapToStudent(StudentMessage studentMessage)
    {
        DateTimeOffset currentDateTime = _dateTimeBroker.GetCurrentDateTime();

        return new Student
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid().ToString(),
            IdentityNumber = studentMessage.IdentityNumber,
            FirstName = studentMessage.FirstName,
            MiddleName = studentMessage.MiddleName,
            LastName = studentMessage.LastName,
            Gender = (Gender)studentMessage.Gender,
            BirthDate = studentMessage.BirthDate,
            CreatedBy = studentMessage.CreatedBy,
            UpdatedBy = studentMessage.CreatedBy,
            CreatedDate = currentDateTime,
            UpdatedDate = currentDateTime
        };
    }
}
