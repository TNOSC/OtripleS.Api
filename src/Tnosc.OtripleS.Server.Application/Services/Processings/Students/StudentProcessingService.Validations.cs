// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Tnosc.OtripleS.Server.Application.Exceptions.Processings.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Processings.Students;

public sealed partial class StudentProcessingService
{
    private static void ValidateStudentOnUpsert(Student student) => 
        ValidateStudent(student);

    private static void ValidateStudent(Student student)
    {
        if (student is null)
        {
            throw new NullStudentProcessingException(message: "The student is null.");
        }
    }
}

