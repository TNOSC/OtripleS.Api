// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using Tnosc.Lib.Domain;

namespace Tnosc.OtripleS.Server.Domain.Students;

public sealed record class StudentId(Guid Value) : IEntityId
{
    public static implicit operator Guid(StudentId studentId) => studentId.Value;
    public static implicit operator StudentId(Guid value) => new(value);
}
