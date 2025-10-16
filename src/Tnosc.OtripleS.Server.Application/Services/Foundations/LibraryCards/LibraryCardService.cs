// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Brokers.Storages;
using Tnosc.OtripleS.Server.Domain.LibraryCards;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryCards;

public class LibraryCardService : ILibraryCardService
{
    private readonly IStorageBroker _storageBroker;

    public LibraryCardService(IStorageBroker storageBroker) =>
        _storageBroker = storageBroker;

    public ValueTask<LibraryCard> AddLibraryCardAsync(LibraryCard libraryCard) =>
        throw new System.NotImplementedException();
}
