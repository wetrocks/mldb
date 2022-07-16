namespace MLDB.Domain;

using System;

internal class PropertyEnforcer {
    internal static Func<DateTime, DateTime> EnforceUtc = x => x.Kind == DateTimeKind.Utc ? x : throw new ArgumentException("Kind must be Utc");
}