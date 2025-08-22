// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.Students;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal partial class StorageBroker
{
    private delegate ValueTask<Student> ReturningStudentFunction();
    private delegate ValueTask<IEnumerable<Student>> ReturningStudentsFunction();

    private static async ValueTask<Student> TryCatch(ReturningStudentFunction returningStudentFunction)
    {
        try
        {
            return await returningStudentFunction();
        }
        catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
        {
            var lockedStudentException =
                new LockedStudentException(
                    message: "Locked student record exception, please try again later.",
                    innerException: dbUpdateConcurrencyException);

            throw lockedStudentException;
        }
        catch (DbUpdateException dbUpdateException)
        {
            var failedStudentStorageException =
                new FailedStudentStorageException(
                    message: "Failed student storage error occurred, contact support.",
                    innerException: dbUpdateException);

            throw failedStudentStorageException;
        }
        catch (DuplicateKeyException duplicateKeyException)
        {
            var alreadyExistsStudentException =
                new AlreadyExistsStudentException(
                    message: "Student with the same id already exists.",
                    innerException: duplicateKeyException);

            throw alreadyExistsStudentException;
        }
    }

    private static async ValueTask<IEnumerable<Student>> TryCatch(ReturningStudentsFunction returningStudentsFunction)
    {
        try
        {
            return await returningStudentsFunction();
        }
        catch (DbUpdateException dbUpdateException)
        {
            var failedStudentStorageException =
                new FailedStudentStorageException(
                    message: "Failed student storage error occurred, contact support.",
                    innerException: dbUpdateException);

            throw failedStudentStorageException;
        }
    }
}
