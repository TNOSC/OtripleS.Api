// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Shouldly;
using Tnosc.OtripleS.Server.Domain.Students;
using Xunit;

namespace Tnosc.OtripleS.Server.Tests.Acceptance.Apis.Students;

public partial class StudentsApiTests
{
    [Fact]
    public async Task ShouldGetAllStudentAsync()
    {
        // given
        IEnumerable<Student> randomStudents = GetRandomStudents();
        IEnumerable<Student> inputStudents = randomStudents;

        foreach (Student student in inputStudents!)
        {
            await _otripleSApiBroker.PostStudentAsync(student);
        }

        var expectedStudents = inputStudents.ToList();

        // when
        List<Student> actualStudents = await _otripleSApiBroker.GetAllStudentsAsync();

        // then
        foreach (Student expectedStudent in expectedStudents)
        {
            Student actualStudent = actualStudents.Single(student => student.Id == expectedStudent.Id);
            actualStudent.ShouldBeEquivalentTo(expectedStudent);
            await _otripleSApiBroker.DeleteStudentByIdAsync(actualStudent.Id);
        }
    }
}
