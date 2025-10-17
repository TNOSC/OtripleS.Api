// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tnosc.OtripleS.Server.Domain.LibraryCards;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages.EntityConfigurations;

internal sealed class LibraryCardConfiguration : IEntityTypeConfiguration<LibraryCard>
{
    public void Configure(EntityTypeBuilder<LibraryCard> builder)
    {
        builder.ToTable("LibraryCards");
        builder.HasKey(la => la.Id);
        builder.Property(s => s.Id)
              .ValueGeneratedNever();

        builder.HasOne(lc => lc.LibraryAccount)
               .WithMany(la => la.LibraryCards)
               .HasForeignKey(lc => lc.LibraryAccountId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
