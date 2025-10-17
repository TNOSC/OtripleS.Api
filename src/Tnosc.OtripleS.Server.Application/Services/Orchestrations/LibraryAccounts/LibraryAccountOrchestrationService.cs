// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryAccounts;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryCards;
using Tnosc.OtripleS.Server.Application.Services.Foundations.LocalStudentEvents;
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;
using Tnosc.OtripleS.Server.Domain.LibraryCards;

namespace Tnosc.OtripleS.Server.Application.Services.Orchestrations.LibraryAccounts;

public class LibraryAccountOrchestrationService : ILibraryAccountOrchestrationService
{
    private readonly ILibraryAccountService _libraryAccountService;
    private readonly ILibraryCardService _libraryCardService;
    private readonly ILocalStudentEventService _localStudentEventService;

    public LibraryAccountOrchestrationService(
        ILibraryAccountService libraryAccountService,
        ILibraryCardService libraryCardService,
        ILocalStudentEventService localStudentEventService)
    {
        _libraryAccountService = libraryAccountService;
        _libraryCardService = libraryCardService;
        _localStudentEventService = localStudentEventService;
    }

    public void ListenToLocalStudentEvent() =>
        _localStudentEventService.ListenToStudentEvent(async (student) =>
        {
            var libraryAccount = new LibraryAccount
            {
                Id = Guid.NewGuid(),
                StudentId = student.Id
            };

            await CreateLibraryAccountAsync(libraryAccount);
        });

    public async ValueTask<LibraryAccount> CreateLibraryAccountAsync(LibraryAccount libraryAccount)
    {
        LibraryAccount addedLibraryAccount = await _libraryAccountService
            .AddLibraryAccountAsync(libraryAccount: libraryAccount);

        await CreateLibraryCardAsync(libraryAccount: libraryAccount);

        return addedLibraryAccount;
    }

    private async Task CreateLibraryCardAsync(LibraryAccount libraryAccount)
    {
        LibraryCard inputLibraryCard =
            CreateLibraryCard(libraryAccountId: libraryAccount.Id);

        await _libraryCardService
            .AddLibraryCardAsync(libraryCard: inputLibraryCard);
    }

    private static LibraryCard CreateLibraryCard(Guid libraryAccountId) =>
        new LibraryCard
        {
            Id = Guid.NewGuid(),
            LibraryAccountId = libraryAccountId
        };
}
