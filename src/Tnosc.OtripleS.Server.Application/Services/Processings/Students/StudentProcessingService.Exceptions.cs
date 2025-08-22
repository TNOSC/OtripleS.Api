// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------


using System;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Application.Exceptions.Processings.Students;
using Tnosc.OtripleS.Server.Domain.Students;
using Xeptions;

namespace Tnosc.OtripleS.Server.Application.Services.Processings.Students;

public sealed partial class StudentProcessingService
{
    private delegate ValueTask<Student> ReturningStudentFunction();

    private async ValueTask<Student> TryCatch(ReturningStudentFunction returningStudentFunction)
    {
        try
        {
            return await returningStudentFunction();
        }
        catch (NullStudentProcessingException nullStudentProcessingException)
        {
            throw CreateAndLogValidationException(exception: nullStudentProcessingException);
        }
        catch (InvalidStudentProcessingException invalidStudentProcessingException)
        {
            throw CreateAndLogValidationException(exception: invalidStudentProcessingException);
        }
        catch (StudentValidationException studentValidationException)
        {
            throw CreateAndLogDependencyValidationException(exception: studentValidationException);
        }
        catch (StudentDependencyValidationException studentValidationException)
        {
            throw CreateAndLogDependencyValidationException(exception: studentValidationException);
        }
    }

    private StudentProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
    {
        var studentProcessingDependencyValidationException = new StudentProcessingDependencyValidationException(
           message: "Student processing dependency validation error occurred, fix the errors and try again.",
           innerException: (exception.InnerException as Xeption)!);
        _loggingBroker.LogError(exception: studentProcessingDependencyValidationException);

        return studentProcessingDependencyValidationException;
    }

    private StudentProcessingValidationException CreateAndLogValidationException(Xeption exception)
    {
        var studentProcessingValidationException = new StudentProcessingValidationException(
            message: "Invalid input, fix the errors and try again.",
            innerException: exception);
        _loggingBroker.LogError(exception: studentProcessingValidationException);

        return studentProcessingValidationException;
    }
}

