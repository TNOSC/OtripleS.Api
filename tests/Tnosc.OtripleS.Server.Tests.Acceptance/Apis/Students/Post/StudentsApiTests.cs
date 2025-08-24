// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Acceptance.Apis.Students;

public partial class StudentsApiTests
{
    [Fact]
    public async Task ShouldPostStudentAsync()
    {
        // given
        Student randomStudent = CreateRandomStudent();
        Student inputStudent = randomStudent;
        Student expectedStudent = inputStudent;

        // when
        Student actualStudent =
            await _otripleSpiBroker.PostStudentAsync(student: inputStudent);

        // then
        actualStudent.ShouldBeEquivalentTo(expected: expectedStudent);
        await _otripleSpiBroker.DeleteStudentByIdAsync(actualStudent.Id);
    }
}
