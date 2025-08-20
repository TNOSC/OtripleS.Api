// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
using NSubstitute;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Unit.Services.Foundations.Students;

public partial class StudentServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllStudentsAsync()
    {
        // given
        IEnumerable<Student> randomStudents = CreateRandomStudents();
        IEnumerable<Student> storageStudents = randomStudents;
        IEnumerable<Student> expectedStudents = storageStudents.DeepClone();

        _storageBrokerMock.SelectAllStudentsAsync()
            .Returns(returnThis: storageStudents);

        // when
        IEnumerable<Student> actualStudents =
            await _studentService.RetrieveAllStudentsAsync();

        // then
        actualStudents.ShouldBeEquivalentTo(expected: expectedStudents);

        await _storageBrokerMock
           .Received(requiredNumberOfCalls: 1)
           .SelectAllStudentsAsync();

        _loggingBrokerMock
            .ReceivedCalls()
            .ShouldBeEmpty();
    }
}
