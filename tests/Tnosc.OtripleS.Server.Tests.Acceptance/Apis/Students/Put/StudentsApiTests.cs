// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Acceptance.Apis.Students;

public partial class StudentsApiTests
{
    [Fact]
    public async Task ShouldGetByIdStudentAsync()
    {
        // given
        Student expectedStudent = await PostRandomStudentAsync();

        // when
        Student actualStudent =
            await _otripleSApiBroker.GetStudentByIdAsync(expectedStudent.Id);

        // then
        actualStudent.ShouldBeEquivalentTo(expectedStudent);
        await _otripleSApiBroker.DeleteStudentByIdAsync(actualStudent.Id);
    }
}
