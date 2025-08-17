// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using NSubstitute;
using Tnosc.OtripleS.Server.Application.Brokers.DateTimes;
using Tnosc.OtripleS.Server.Application.Brokers.Loggings;
using Tnosc.OtripleS.Server.Application.Brokers.Storages;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Tynamix.ObjectFiller;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTest
{
    private readonly IStorageBroker _storageBrokerMock;
    private readonly IDateTimeBroker _dateTimeBrokerMock;
    private readonly ILoggingBroker _loggingBrokerMock;
    private readonly StudentService _studentService;

    public StudentServiceTest()
    {
        _storageBrokerMock = Substitute.For<IStorageBroker>();
        _dateTimeBrokerMock = Substitute.For<IDateTimeBroker>();
        _loggingBrokerMock = Substitute.For<ILoggingBroker>();

        _studentService = new StudentService(
            storageBroker: _storageBrokerMock,
            dateTimeBroker: _dateTimeBrokerMock,
            loggingBroker: _loggingBrokerMock);
    }

    private static DateTimeOffset GetRandomDateTime() =>
           new DateTimeRange(earliestDate: DateTime.UtcNow).GetValue();
    
    private static Student CreateRandomStudent(DateTimeOffset date) =>
           CreateStudentFiller(date).Create();

    private static Filler<Student> CreateStudentFiller(DateTimeOffset date)
    {
        var filler = new Filler<Student>();
        var createdById = Guid.NewGuid();

        filler.Setup()
            .OnProperty(student => student.BirthDate).Use(GetRandomDateTime())
            .OnProperty(student => student.CreatedDate).Use(date)
            .OnProperty(student => student.UpdatedDate).Use(date)
            .OnProperty(student => student.CreatedBy).Use(createdById)
            .OnProperty(student => student.UpdatedBy).Use(createdById);

        return filler;
    }

    private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
    private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();

    public static TheoryData InvalidMinuteCases()
    {
        int randomMoreThanMinuteFromNow = GetRandomNumber();
        int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

        return new TheoryData<int>
        {
            randomMoreThanMinuteFromNow ,
            randomMoreThanMinuteBeforeNow
        };
    }


    private static SqlException CreateSqlException(int errorCode)
    {
        Exception? innerEx = null;
        ConstructorInfo[] constructorInfo = typeof(SqlErrorCollection).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
        SqlErrorCollection errors = (constructorInfo[0].Invoke(null) as SqlErrorCollection)!;
        List<object> errorList = (errors.GetType().GetField("_errors", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(errors) as List<object>)!;
        constructorInfo = typeof(SqlError).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
        ConstructorInfo nineC = constructorInfo.FirstOrDefault(f => f.GetParameters().Length == 9)!;
        SqlError sqlError = (nineC.Invoke(new object?[] { errorCode, (byte)0, (byte)0, "", "", "", 0, (uint)0, innerEx }) as SqlError)!;
        errorList.Add(sqlError);
        SqlException ex = (Activator.CreateInstance(typeof(SqlException), BindingFlags.NonPublic | BindingFlags.Instance, null, new object?[] { "test", errors,
            innerEx, Guid.NewGuid() }, null) as SqlException)!;
        return ex;
    }
}
