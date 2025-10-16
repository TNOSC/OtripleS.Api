// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Tnosc.OtripleS.Server.Domain.LibraryCards;

namespace Tnosc.OtripleS.Server.Application.Services.Foundations.LibraryCards;

public interface ILibraryCardService
{
    ValueTask<LibraryCard> AddLibraryCardAsync(LibraryCard libraryCard);
}
