// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

namespace Tnosc.Lib.Domain;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{

    public required TId Id { get; init; }

    public byte[] RowVersion { get; private set; } = null!;

    public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId>? a, Entity<TId>? b) => !(a == b);

    public virtual bool Equals(Entity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Entity<TId> other)
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => HashCode.Combine(Id);
}
