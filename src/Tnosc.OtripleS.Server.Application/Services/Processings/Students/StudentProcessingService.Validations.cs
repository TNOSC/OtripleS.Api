// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using Tnosc.OtripleS.Server.Application.Exceptions.Processings.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Application.Services.Processings.Students;

public sealed partial class StudentProcessingService
{
    private static void ValidateStudentOnUpsert(Student student)
    {
        ValidateStudent(student);
        Validate((Rule: IsInvalid(student.Id), Parameter: nameof(Student.Id)));
    }

    private static void ValidateStudent(Student student)
    {
        if (student is null)
        {
            throw new NullStudentProcessingException(message: "The student is null.");
        }
    }

    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };

    private static void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidPostImpressionProcessingException =
            new InvalidStudentProcessingException(message: "Invalid student. Please fix the errors and try again.");

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidPostImpressionProcessingException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidPostImpressionProcessingException.ThrowIfContainsErrors();
    }
}

