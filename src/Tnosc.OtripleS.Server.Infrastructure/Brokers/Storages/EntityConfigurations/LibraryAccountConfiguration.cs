// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tnosc.OtripleS.Server.Domain.Libraries;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages.EntityConfigurations;

internal sealed class LibraryAccountConfiguration : IEntityTypeConfiguration<LibraryAccount>
{
    public void Configure(EntityTypeBuilder<LibraryAccount> builder)
    {
        builder.ToTable("LibraryAccounts");
        builder.HasKey(la => la.Id);
        builder.Property(s => s.Id)
              .ValueGeneratedNever();

        builder.HasOne(la => la.Student)
               .WithOne(st => st.LibraryAccount)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
