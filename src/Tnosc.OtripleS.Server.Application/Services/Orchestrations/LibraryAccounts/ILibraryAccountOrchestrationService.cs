// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;

namespace Tnosc.OtripleS.Server.Application.Services.Orchestrations.LibraryAccounts;

public interface ILibraryAccountOrchestrationService
{
    ValueTask<LibraryAccount> CreateLibraryAccountAsync(LibraryAccount libraryAccount);
}
