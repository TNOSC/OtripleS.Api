// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Acceptance.Apis.Students;

public partial class StudentsApiTests
{
    [Fact]
    public async Task ShouldDeleteStudentAsync()
    {
        // given
        Student randomStudent = await PostRandomStudentAsync();
        Student inputStudent = randomStudent;
        Student expectedStudent = inputStudent;

        // when 
        Student deletedStudent =
            await _otripleSApiBroker.DeleteStudentByIdAsync(inputStudent.Id);

        ValueTask<Student> getStudentByIdTask =
            _otripleSApiBroker.GetStudentByIdAsync(inputStudent.Id);

        // then
        deletedStudent.ShouldBeEquivalentTo(expectedStudent);

        await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
           getStudentByIdTask.AsTask());
    }
}
