// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Brokers.Storages;
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryAccounts;

public sealed class LibraryAccountService : ILibraryAccountService
{
    private readonly IStorageBroker _storageBroker;

    public LibraryAccountService(IStorageBroker storageBroker) =>
        _storageBroker = storageBroker;

    public ValueTask<LibraryAccount> AddLibraryAccountAsync(LibraryAccount libraryAccount) =>
        throw new NotImplementedException();
}
