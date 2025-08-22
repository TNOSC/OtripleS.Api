// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using NSubstitute;
using Tnosc.OtripleS.Server.Application.Brokers.Loggings;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Processings.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Processings.Students;

public partial class StudentProcessingServiceTests
{
    private readonly ILoggingBroker _loggingBrokerMock;
    private readonly IStudentService _studentServiceMock;
    private readonly StudentProcessingService _studentProcessingService;
    public StudentProcessingServiceTests()
    {
        _loggingBrokerMock = Substitute.For<ILoggingBroker>();
        _studentServiceMock = Substitute.For<IStudentService>();
        _studentProcessingService = new StudentProcessingService(
            studentService: _studentServiceMock,
            loggingBroker: _loggingBrokerMock);
    }

    private static Student CreateRandomStudent() =>
         CreateStudentFiller(date: DateTimeOffset.UtcNow).Create();

    private static Filler<Student> CreateStudentFiller(DateTimeOffset date)
    {
        var filler = new Filler<Student>();
        var createdById = Guid.NewGuid();

        filler.Setup()
            .OnProperty(student => student.BirthDate).Use(valueToUse: GetRandomDateTime())
            .OnProperty(student => student.CreatedDate).Use(valueToUse: date)
            .OnProperty(student => student.UpdatedDate).Use(valueToUse: date)
            .OnProperty(student => student.CreatedBy).Use(valueToUse: createdById)
            .OnProperty(student => student.UpdatedBy).Use(valueToUse: createdById);

        return filler;
    }

    private static DateTimeOffset GetRandomDateTime() =>
          new DateTimeRange(earliestDate: DateTime.UtcNow).GetValue();

    private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();
    private static int GetRandomNumber() =>
          new IntRange(min: 2, max: 10).GetValue();

    public static TheoryData DependencyValidationExceptions()
    {
        string randomMessage = GetRandomMessage();
        string exceptionMessage = randomMessage;
        var innerException = new Xeption(message: exceptionMessage);

        return new TheoryData<Xeption>
        {
            new StudentValidationException(
                message: "Invalid input, fix the errors and try again.",
                innerException: innerException),
            new StudentDependencyValidationException(
                message: "Student dependency validation error occurred, fix the errors and try again.",
                innerException: innerException)
        };
    }
}
