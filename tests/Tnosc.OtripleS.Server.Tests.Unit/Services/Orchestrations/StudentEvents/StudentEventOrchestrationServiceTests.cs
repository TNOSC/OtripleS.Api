// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using NSubstitute;
using Tnosc.OtripleS.Server.Application.Brokers.DateTimes;
using Tnosc.OtripleS.Server.Application.Brokers.Queues.Messages;
using Tnosc.OtripleS.Server.Application.Services.Foundations.StudentEvents;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Orchestrations;
using Tnosc.OtripleS.Server.Application.Services.Orchestrations.StudentEvents;
using Tnosc.OtripleS.Server.Domain.Students;
using Tynamix.ObjectFiller;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Orchestrations.StudentEvents;

public partial class StudentEventOrchestrationServiceTests
{
    private readonly IStudentService _studentServiceMock;
    private readonly IStudentEventService _studentEventServiceMock;
    private readonly IDateTimeBroker _dateTimeBrokerMock;
    private readonly IStudentEventOrchestrationService _studentEventOrchestrationService;

    public StudentEventOrchestrationServiceTests()
    {
        _studentServiceMock = Substitute.For<IStudentService>();
        _studentEventServiceMock = Substitute.For<IStudentEventService>();
        _dateTimeBrokerMock = Substitute.For<IDateTimeBroker>();

        _studentEventOrchestrationService = new StudentEventOrchestrationService(
            studentService: _studentServiceMock,
            studentEventService: _studentEventServiceMock,
            dateTimeBroker: _dateTimeBrokerMock);
    }
    private static dynamic CreateRandomStudentProperties(
       DateTimeOffset auditDates,
       Guid auditIds)
    {
        Gender randomStudentGender = GetRandomGender();

        return new
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid().ToString(),
            IdentityNumber = GetRandomString(),
            FirstName = GetRandomName(),
            MiddleName = GetRandomName(),
            LastName = GetRandomName(),
            BirthDate = GetRandomDate(),
            Gender = randomStudentGender,
            GenderMessage = (StudentGenderMessage)randomStudentGender,
            CreatedDate = auditDates,
            UpdatedDate = auditDates,
            CreatedBy = auditIds,
            UpdatedBy = auditIds
        };
    }

    private static Gender GetRandomGender()
    {
        int studentGenderCount =
            Enum.GetValues<Gender>().Length;

        int randomStudentGenderValue =
            new IntRange(
                min: 0,
                max: studentGenderCount).GetValue();

        return (Gender)randomStudentGenderValue;
    }

    private static string GetRandomString() =>
        new MnemonicString().GetValue();

    private static string GetRandomName() =>
        new RealNames(NameStyle.FirstName).GetValue();

    private static DateTimeOffset GetRandomDate() =>
        new DateTimeRange(earliestDate: DateTime.UtcNow).GetValue();

    private static bool SameStudentAs(Student actualStudent, Student expectedStudent) =>
       actualStudent.IdentityNumber == expectedStudent.IdentityNumber &&
       actualStudent.FirstName == expectedStudent.FirstName &&
       actualStudent.MiddleName == expectedStudent.MiddleName &&
       actualStudent.LastName == expectedStudent.LastName &&
       actualStudent.Gender == expectedStudent.Gender &&
       actualStudent.BirthDate == expectedStudent.BirthDate &&
       actualStudent.CreatedDate == expectedStudent.CreatedDate &&
       actualStudent.UpdatedDate == expectedStudent.UpdatedDate &&
       actualStudent.CreatedBy == expectedStudent.CreatedBy &&
       actualStudent.UpdatedBy == expectedStudent.UpdatedBy;
}
