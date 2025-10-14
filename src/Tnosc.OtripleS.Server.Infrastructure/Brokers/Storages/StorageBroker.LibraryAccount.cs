// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tnosc.OtripleS.Server.Domain.Libraries;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal partial class StorageBroker
{
    public DbSet<LibraryAccount> LibraryAccounts { get; set; }

    public async ValueTask<LibraryAccount> InsertLibraryAccountAsync(LibraryAccount libraryAccount) => 
        await TryCatch(async () => await InsertAsync(@object: libraryAccount));
}
