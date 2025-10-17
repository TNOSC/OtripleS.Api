// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tnosc.OtripleS.Server.Application.Exceptions.Foundations.LibraryCards;
using Tnosc.OtripleS.Server.Domain.LibraryCards;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal partial class StorageBroker
{
    private delegate ValueTask<LibraryCard> ReturningLibraryCardFunction();

    private static async ValueTask<LibraryCard> TryCatch(ReturningLibraryCardFunction returningLibraryCardFunction)
    {
        try
        {
            return await returningLibraryCardFunction();
        }
        catch (SqlException sqlException)
        {
            var failedLibraryCardStorageException =
                new FailedLibraryCardStorageException(
                    message: "Failed LibraryCard storage error occurred, contact support.",
                    innerException: sqlException);

            throw failedLibraryCardStorageException;
        }
        catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
        {
            var lockedLibraryCardException =
                new LockedLibraryCardException(
                    message: "Locked LibraryCard record exception, please try again later.",
                    innerException: dbUpdateConcurrencyException);

            throw lockedLibraryCardException;
        }
        catch (DuplicateKeyException duplicateKeyException)
        {
            var alreadyExistsLibraryCardException =
                new AlreadyExistsLibraryCardException(
                    message: "LibraryCard with the same id already exists.",
                    innerException: duplicateKeyException);

            throw alreadyExistsLibraryCardException;
        }
        catch (DbUpdateException dbUpdateException)
        {
            var failedLibraryCardStorageException =
                new FailedLibraryCardStorageException(
                    message: "Failed LibraryCard storage error occurred, contact support.",
                    innerException: dbUpdateException);

            throw failedLibraryCardStorageException;
        }
    }
}
