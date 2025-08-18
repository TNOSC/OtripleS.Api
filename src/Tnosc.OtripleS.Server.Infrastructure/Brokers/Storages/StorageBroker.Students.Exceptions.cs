// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Tnosc.OtripleS.Server.Domain.Students;
using Tnosc.OtripleS.Server.Domain.Students.Exceptions;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal partial class StorageBroker
{
    private delegate ValueTask<Student> ReturningStudentFunction();

    private static async ValueTask<Student> TryCatch(ReturningStudentFunction returningStudentFunction)
    {
        try
        {
            return await returningStudentFunction();
        }
        catch (SqlException sqlException)
        {
            var failedStudentStorageException =
                new FailedStudentStorageException(
                    message: "Failed student storage error occurred, contact support.",
                    innerException: sqlException);

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
}
