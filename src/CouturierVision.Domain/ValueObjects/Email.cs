using System.Text.RegularExpressions;
using CouturierVision.Domain.Exceptions;

namespace CouturierVision.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");
        if (!EmailRegex.IsMatch(value))
            throw new DomainException($"'{value}' is not a valid email address.");
        Value = value.ToLowerInvariant();
    }

    public bool Equals(Email? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Email email && Equals(email);
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override string ToString() => Value;

    public static bool operator ==(Email? left, Email? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(Email? left, Email? right) => !(left == right);

    public static implicit operator string(Email email) => email.Value;
}
