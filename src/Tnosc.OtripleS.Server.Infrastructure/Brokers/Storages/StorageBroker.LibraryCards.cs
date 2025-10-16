// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tnosc.OtripleS.Server.Domain.LibraryCards;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal partial class StorageBroker
{
    public DbSet<LibraryCard> LibraryCards { get; set; }

    public async ValueTask<LibraryCard> InsertLibraryCardAsync(LibraryCard libraryCard) =>
       await TryCatch(async () => await InsertAsync(@object: libraryCard));
}
