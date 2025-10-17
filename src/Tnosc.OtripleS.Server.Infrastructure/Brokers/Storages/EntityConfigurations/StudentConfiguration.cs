// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tnosc.OtripleS.Server.Domain.Students;

namespace Tnosc.OtripleS.Server.Infrastructure.Brokers.Storages.EntityConfigurations;

internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
               .ValueGeneratedNever();

        builder.Property(s => s.UserId)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.IdentityNumber)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(s => s.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.MiddleName)
               .HasMaxLength(100);

        builder.Property(s => s.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.BirthDate)
               .IsRequired();

        builder.Property(s => s.Gender)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(10);

        builder.Property(s => s.CreatedDate)
               .IsRequired();

        builder.Property(s => s.UpdatedDate)
               .IsRequired();

        builder.Property(s => s.CreatedBy)
               .IsRequired();

        builder.Property(s => s.UpdatedBy)
               .IsRequired();
    }
}
