// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Domain.Students.Exceptions;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

public sealed partial class StudentService
{
    private static void ValidateStudentOnRegister(Student student) =>
        ValidateStudent(student);

    private static void ValidateStudent(Student student)
    {
        if (student is null)
        {
            throw new NullStudentException();
        }
    }
}
