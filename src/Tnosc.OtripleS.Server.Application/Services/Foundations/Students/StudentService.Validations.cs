// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Data;
using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Domain.Students.Exceptions;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

public sealed partial class StudentService
{
    private void ValidateStudentOnRegister(Student student)
    {
        ValidateStudent(student);

        Validate(
            (Rule: IsInvalid(student.Id), Parameter: nameof(Student.Id)),
            (Rule: IsInvalid(student.UserId), Parameter: nameof(Student.UserId)),
            (Rule: IsInvalid(student.IdentityNumber), Parameter: nameof(Student.IdentityNumber)),
            (Rule: IsInvalid(student.FirstName), Parameter: nameof(Student.FirstName)),
            (Rule: IsInvalid(student.BirthDate), Parameter: nameof(Student.BirthDate)),
            (Rule: IsInvalid(student.CreatedBy), Parameter: nameof(Student.CreatedBy)),
            (Rule: IsInvalid(student.UpdatedBy), Parameter: nameof(Student.UpdatedBy)),
            (Rule: IsInvalid(student.CreatedDate), Parameter: nameof(Student.CreatedDate)),
            (Rule: IsInvalid(student.UpdatedDate), Parameter: nameof(Student.UpdatedDate)),
            (Rule: IsNotRecent(student.CreatedDate), Parameter: nameof(Student.CreatedDate)),

            (Rule: IsNotSame(
                firstId: student.UpdatedBy,
                secondId: student.CreatedBy,
                secondIdName: nameof(Student.CreatedBy)),
                Parameter: nameof(Student.UpdatedBy)),

            (Rule: IsNotSame(
                firstDate: student.UpdatedDate,
                secondDate: student.CreatedDate,
                secondDateName: nameof(Student.CreatedDate)),
                Parameter: nameof(Student.UpdatedDate))
        );
    }

    private static void ValidateStudent(Student student)
    {
        if (student is null)
        {
            throw new NullStudentException();
        }
    }

    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };

    private static dynamic IsInvalid(string text) => new
    {
        Condition = string.IsNullOrWhiteSpace(text),
        Message = "Text is required"
    };

    private static dynamic IsInvalid(DateTimeOffset date) => new
    {
        Condition = date == default,
        Message = "Date is required"
    };

    private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
    {
        Condition = IsDateNotRecent(dateTimeOffset),
        Message = "Date is not recent"
    };

    private bool IsDateNotRecent(DateTimeOffset date)
    {
        DateTimeOffset currentDateTime =
            _dateTimeBroker.GetCurrentDateTime();

        TimeSpan timeDifference = currentDateTime.Subtract(date);
        var oneMinute = TimeSpan.FromMinutes(1);

        return timeDifference.Duration() > oneMinute;
    }

    private static dynamic IsNotSame(
          Guid firstId,
          Guid secondId,
          string secondIdName) => new
          {
              Condition = firstId != secondId,
              Message = $"Id is not the same as {secondIdName}"
          };

    private static dynamic IsNotSame(
        DateTimeOffset firstDate,
        DateTimeOffset secondDate,
        string secondDateName) => new
        {
            Condition = firstDate != secondDate,
            Message = $"Date is not the same as {secondDateName}"
        };

    private static void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidStudentException = new InvalidStudentException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidStudentException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidStudentException.ThrowIfContainsErrors();
    }
}
