// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Acceptance.Apis.Students;

public partial class StudentsApiTests
{
    [Fact]
    public async Task ShouldPutStudentAsync()
    {
        // given
        Student randomStudent = await PostRandomStudentAsync();
        Student modifiedStudent = UpdateStudentRandom(randomStudent);

        // when
        await _otripleSApiBroker.PutStudentAsync(modifiedStudent);

        Student actualStudent =
            await _otripleSApiBroker.GetStudentByIdAsync(randomStudent.Id);

        // then
        actualStudent.Should().BeEquivalentTo(modifiedStudent);
        await _otripleSApiBroker.DeleteStudentByIdAsync(actualStudent.Id);
    }
}
