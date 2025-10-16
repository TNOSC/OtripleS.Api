// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using Tnosc.OtripleS.Server.Domain.LibraryAccounts;

namespace Tnosc.OtripleS.Server.Domain.LibraryCards;

public sealed class LibraryCard
{
    public Guid Id { get; set; }


    // Navigations
    public Guid LibraryAccountId { get; set; }
    public LibraryAccount LibraryAccount { get; set; } = null!;
}
