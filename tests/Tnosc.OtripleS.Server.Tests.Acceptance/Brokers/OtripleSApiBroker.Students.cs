// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Tests.Acceptance.Brokers;

public partial class OtripleSApiBroker
{
    private const string StudentsRelativeUrl = "api/students";

    public async ValueTask<Student> PostStudentAsync(Student student) =>
        await _apiFactoryClient.PostContentAsync(StudentsRelativeUrl, student);


    public async ValueTask<Student> DeleteStudentByIdAsync(Guid studentId) =>
           await _apiFactoryClient.DeleteContentAsync<Student>($"{StudentsRelativeUrl}/{studentId}");
}

