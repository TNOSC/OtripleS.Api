// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Xeptions;

namespace Tnosc.OtripleS.Server.Domain.Students.Exceptions;

public sealed class NullStudentException : Xeption
{
    public NullStudentException()
        : base(message: "The student is null.")
    { }

    public NullStudentException(string message)
        : base(message: message)
    { }
}
