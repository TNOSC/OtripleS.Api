// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NSubstitute;
using RESTFulSense.Controllers;
using Tnosc.OtripleS.Server.Api.Endpoints.Students;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Services.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;


namespace Tnosc.OtripleS.Server.Tests.Unit.Enpoints.Students.Post;

public partial class PostStudentEndpointTests : RESTFulController
{
    private readonly IStudentService _studentService;
    private readonly PostStudentEndpoint _postStudentEndpoint;

    public PostStudentEndpointTests()
    {
        _studentService = Substitute.For<IStudentService>();
        _postStudentEndpoint = new PostStudentEndpoint(studentService: _studentService);
    }

    public static TheoryData<Xeption> ValidationExceptions()
    {
        var someInnerException = new Xeption();
        string someMessage = GetRandomString();

        return
        [
            new StudentValidationException(
                message: someMessage,
                innerException: someInnerException),

            new StudentDependencyValidationException(
                message: someMessage,
                innerException: someInnerException)
        ];
    }

    public static TheoryData<Xeption> ServerExceptions()
    {
        var someInnerException = new Xeption();
        string someMessage = GetRandomString();

        return
        [
            new StudentDependencyException(
                message: someMessage,
                innerException: someInnerException),

            new StudentServiceException(
                message: someMessage,
                innerException: someInnerException)
        ];
    }

    private static Student CreateRandomStudent() =>
       CreateStudentFiller(date: DateTimeOffset.UtcNow).Create();

    private static DateTimeOffset GetRandomDateTime() =>
       new DateTimeRange(earliestDate: DateTime.UtcNow).GetValue();

    private static string GetRandomString() =>
            new MnemonicString().GetValue();

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
}
