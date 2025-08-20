// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
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
        catch (InvalidStudentException invalidStudentException)
        {
            throw CreateAndLogValidationException(invalidStudentException);
        }
        catch (NotFoundStudentException notFoundSutdentException)
        {
            throw CreateAndLogValidationException(notFoundSutdentException);
        }
        catch (FailedStudentStorageException failedStudentStorageException)
        {
            throw CreateAndLogCriticalDependencyException(failedStudentStorageException);
        }
        catch (StudentConcurrencyStorageException studentConcurrencyStorageException)
        {
            throw CreateAndLogDependencyValidationException(studentConcurrencyStorageException);
        }
        catch (AlreadyExistsStudentException alreadyExistsStudentException)
        {
            throw CreateAndLogDependencyValidationException(alreadyExistsStudentException);
        }
        catch(Exception exception)
        {
            var failedStudentServiceException =
                   new FailedStudentServiceException(
                       message: "Failed student service error occurred, contact support.",
                       innerException: exception);

            throw CreateAndLogServiceException(failedStudentServiceException);
        }
    }

    private StudentServiceException CreateAndLogServiceException(Exception exception)
    {
        var studentServiceException = new StudentServiceException(
            message: "Service error occurred, contact support.",
            innerException: exception);
        _loggingBroker.LogError(exception: studentServiceException);

        return studentServiceException;
    }

    private StudentDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
    {
        var studentDependencyValidationException =
            new StudentDependencyValidationException(
                message: "Student dependency validation error occurred, fix the errors and try again.",
                innerException: exception);

        _loggingBroker.LogError(exception: studentDependencyValidationException);

        return studentDependencyValidationException;
    }

    private StudentDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
    {
        var studentDependencyException = new StudentDependencyException(
            message: "Student dependency error occurred, contact support.",
            innerException: exception);
        _loggingBroker.LogCritical(exception: studentDependencyException);

        return studentDependencyException;
    }

    private StudentValidationException CreateAndLogValidationException(Xeption exception)
    {
        var studentValidationException = new StudentValidationException(
            message: "Invalid input, fix the errors and try again.",
            innerException: exception);
        _loggingBroker.LogError(exception: studentValidationException);

        return studentValidationException;
    }
}
