using Newtonsoft.Json.Linq;

using NodaTime;

namespace PluralKit.Core;

public class SystemConfig
{
    public SystemId Id { get; }
    public string UiTz { get; set; }
    public bool PingsEnabled { get; }
    public int? LatchTimeout { get; }
    public bool MemberDefaultPrivate { get; }
    public bool GroupDefaultPrivate { get; }
    public bool ShowPrivateInfo { get; }
    public int? MemberLimitOverride { get; }
    public int? GroupLimitOverride { get; }
    public ICollection<string> DescriptionTemplates { get; }

    public DateTimeZone Zone => DateTimeZoneProviders.Tzdb.GetZoneOrNull(UiTz);

    public bool CaseSensitiveProxyTags { get; set; }
    public bool ProxyErrorMessageEnabled { get; }

    public (IEnumerable<ulong> Users, IEnumerable<ulong> Guilds) Trusted { get; set; }
}

public static class SystemConfigExt
{
    public static JObject ToJson(this SystemConfig cfg)
    {
        var o = new JObject();

        o.Add("timezone", cfg.UiTz);
        o.Add("pings_enabled", cfg.PingsEnabled);
        o.Add("latch_timeout", cfg.LatchTimeout);
        o.Add("member_default_private", cfg.MemberDefaultPrivate);
        o.Add("group_default_private", cfg.GroupDefaultPrivate);
        o.Add("show_private_info", cfg.ShowPrivateInfo);
        o.Add("member_limit", cfg.MemberLimitOverride ?? Limits.MaxMemberCount);
        o.Add("group_limit", cfg.GroupLimitOverride ?? Limits.MaxGroupCount);
        o.Add("case_sensitive_proxy_tags", cfg.CaseSensitiveProxyTags);
        o.Add("proxy_error_message_enabled", cfg.ProxyErrorMessageEnabled);

        var t = new JObject();
        t.Add("users", JArray.FromObject(cfg.Trusted.Users));
        t.Add("guilds", JArray.FromObject(cfg.Trusted.Guilds));
        o.Add("trusted", t);

        o.Add("description_templates", JArray.FromObject(cfg.DescriptionTemplates));

        return o;
    }
}