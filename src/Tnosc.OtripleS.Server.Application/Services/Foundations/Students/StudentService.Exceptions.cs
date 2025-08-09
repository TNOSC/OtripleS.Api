// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Domain.Students.Exceptions;
using Xeptions;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.Students;

public sealed partial class StudentService
{
    private delegate ValueTask<Student> ReturningStudentFunction();
    private async ValueTask<Student> TryCatch(ReturningStudentFunction returningStudentFunction)
    {
        try
        {
            return await returningStudentFunction();
        }
        catch (NullStudentException nullStudentException)
        {
            throw CreateAndLogValidationException(nullStudentException);
        }
    }

    private StudentValidationException CreateAndLogValidationException(Xeption exception)
    {
        var studentValidationException = new StudentValidationException(exception);
        _loggingBroker.LogError(studentValidationException);

        return studentValidationException;
    }
}
