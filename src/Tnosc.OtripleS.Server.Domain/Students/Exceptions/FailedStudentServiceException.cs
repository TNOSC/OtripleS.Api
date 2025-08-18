// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using Xeptions;

namespace Tnosc.OtripleS.Server.Domain.Students.Exceptions;

public sealed class FailedStudentServiceException : Xeption
{
    public FailedStudentServiceException(
        string message,
        Exception innerException)
        : base(
            message: message,
            innerException: innerException)
    { }
}



