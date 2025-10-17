// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Tnosc.OtripleS.Server.Domain.LibraryCards;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Domain.LibraryAccounts;

public sealed class LibraryAccount
{
    public LibraryAccount() => 
        LibraryCards = new HashSet<LibraryCard>();

    public Guid Id { get; set; }


    // Navigations
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public ICollection<LibraryCard> LibraryCards { get; set; }
}
