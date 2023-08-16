namespace PluralKit.Core;

public enum PrivacyLevel
{
    Public = 1,
    Private = 2,
    Trusted = 4,
}

public static class PrivacyLevelExt
{
    public static bool CanAccess(this PrivacyLevel level, LookupContext ctx)
    {
        switch (level)
        {
            case PrivacyLevel.Private:
                return ctx == LookupContext.ByOwner;
            case PrivacyLevel.Public:
                return true;
            case PrivacyLevel.Trusted:
                return ctx == LookupContext.ByOwner || ctx == LookupContext.ByTrusted;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static string LevelName(this PrivacyLevel level) =>
        level switch
        {
            PrivacyLevel.Private => "private",
            PrivacyLevel.Public => "public",
            PrivacyLevel.Trusted => "trusted",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

    public static T Get<T>(this PrivacyLevel level, LookupContext ctx, T input, T fallback = default) =>
        level.CanAccess(ctx) ? input : fallback;

    public static string Explanation(this PrivacyLevel level) =>
        level switch
        {
            PrivacyLevel.Private => "**Private** (visible only when queried by you)",
            PrivacyLevel.Public => "**Public** (visible to everyone)",
            PrivacyLevel.Trusted => "**Trusted** (visible to trusted users and in trusted servers)",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

    public static bool TryGet<T>(this PrivacyLevel level, LookupContext ctx, T input, out T output,
                                 T absentValue = default)
    {
        output = default;
        if (!level.CanAccess(ctx))
            return false;
        if (Equals(input, absentValue))
            return false;

        output = input;
        return true;
    }

    public static string ToJsonString(this PrivacyLevel level) => level.LevelName();
}