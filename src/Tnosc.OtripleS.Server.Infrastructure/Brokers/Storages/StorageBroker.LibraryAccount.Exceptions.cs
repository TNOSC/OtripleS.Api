// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.LibraryAccounts;
using Tnosc.OtripleS.Server.Domain.Libraries;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal partial class StorageBroker
{
    private delegate ValueTask<LibraryAccount> ReturningLibraryAccountFunction();

    private static async ValueTask<LibraryAccount> TryCatch(ReturningLibraryAccountFunction returningLibraryAccountFunction)
    {
        try
        {
            return await returningLibraryAccountFunction();
        }
        catch (SqlException sqlException)
        {
            var failedLibraryAccountStorageException =
                new FailedLibraryAccountStorageException(
                    message: "Failed LibraryAccount storage error occurred, contact support.",
                    innerException: sqlException);

            throw failedLibraryAccountStorageException;
        }
        catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
        {
            var lockedLibraryAccountException =
                new LockedLibraryAccountException(
                    message: "Locked LibraryAccount record exception, please try again later.",
                    innerException: dbUpdateConcurrencyException);

            throw lockedLibraryAccountException;
        }
        catch (DuplicateKeyException duplicateKeyException)
        {
            var alreadyExistsLibraryAccountException =
                new AlreadyExistsLibraryAccountException(
                    message: "LibraryAccount with the same id already exists.",
                    innerException: duplicateKeyException);

            throw alreadyExistsLibraryAccountException;
        }
        catch (DbUpdateException dbUpdateException)
        {
            var failedLibraryAccountStorageException =
                new FailedLibraryAccountStorageException(
                    message: "Failed LibraryAccount storage error occurred, contact support.",
                    innerException: dbUpdateException);

            throw failedLibraryAccountStorageException;
        }
    }
}
