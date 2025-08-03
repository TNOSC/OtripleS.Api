// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages;

internal partial class StorageBroker
{
    public DbSet<Student> Students { get; set; }
}
