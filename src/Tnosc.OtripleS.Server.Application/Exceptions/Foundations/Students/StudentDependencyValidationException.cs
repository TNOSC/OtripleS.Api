// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Xeptions;

namespace Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;

public sealed class StudentDependencyValidationException : Xeption
{
    public StudentDependencyValidationException(
        string message,
        Xeption innerException)
       : base(
           message: message,
           innerException: innerException)
    { }
}
