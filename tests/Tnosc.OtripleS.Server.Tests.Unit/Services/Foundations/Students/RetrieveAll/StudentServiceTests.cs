// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Linq;
using Force.DeepCloner;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTests
{
    [Fact]
    public void ShouldRetrieveAllStudents()
    {
        // given
        IQueryable<Student> randomStudents = CreateRandomStudents();
        IQueryable<Student> storageStudents = randomStudents;
        IQueryable<Student> expectedStudents = storageStudents.DeepClone();

        _storageBrokerMock.SelectAllStudents()
            .Returns(returnThis: storageStudents);

        // when
        IQueryable<Student> actualStudents =
            _studentService.RetrieveAllStudents();

        // then
        actualStudents.ShouldBeEquivalentTo(expected: expectedStudents);

        _storageBrokerMock
            .Received(requiredNumberOfCalls: 1)
            .SelectAllStudents();

        _loggingBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }
}
