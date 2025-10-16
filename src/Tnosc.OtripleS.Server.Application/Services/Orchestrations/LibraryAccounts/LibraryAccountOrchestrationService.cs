// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryAccounts;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryCards;
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;

namespace Tnosc.OtripleS.Server.Application.Services.Orchestrations.LibraryAccounts;

public class LibraryAccountOrchestrationService : ILibraryAccountOrchestrationService
{
#pragma warning disable S4487 // Unread "private" fields should be removed
    private readonly ILibraryAccountService _libraryAccountService;
    private readonly ILibraryCardService _libraryCardService;
#pragma warning restore S4487 // Unread "private" fields should be removed

    public LibraryAccountOrchestrationService(
        ILibraryAccountService libraryAccountService,
        ILibraryCardService libraryCardService)
    {
        _libraryAccountService = libraryAccountService;
        _libraryCardService = libraryCardService;
    }

    public ValueTask<LibraryAccount> CreateLibraryAccountAsync(LibraryAccount libraryAccount) =>
        throw new System.NotImplementedException();
}
