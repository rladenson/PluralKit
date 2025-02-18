namespace PluralKit.Core;

public enum MemberPrivacySubject
{
    Visibility,
    Name,
    Description,
    Banner,
    Avatar,
    Birthday,
    Pronouns,
    Proxy,
    Metadata
}

public static class MemberPrivacyUtils
{
    public static MemberPatch WithPrivacy(this MemberPatch member, MemberPrivacySubject subject, PrivacyLevel level)
    {
        // what do you mean switch expressions can't be statements >.>
        _ = subject switch
        {
            MemberPrivacySubject.Name => member.NamePrivacy = level,
            MemberPrivacySubject.Description => member.DescriptionPrivacy = level,
            MemberPrivacySubject.Banner => member.BannerPrivacy = level,
            MemberPrivacySubject.Avatar => member.AvatarPrivacy = level,
            MemberPrivacySubject.Pronouns => member.PronounPrivacy = level,
            MemberPrivacySubject.Birthday => member.BirthdayPrivacy = level,
            MemberPrivacySubject.Metadata => member.MetadataPrivacy = level,
            MemberPrivacySubject.Proxy => member.ProxyPrivacy = level,
            MemberPrivacySubject.Visibility => member.Visibility = level,
            _ => throw new ArgumentOutOfRangeException($"Unknown privacy subject {subject}")
        };

        return member;
    }

    public static MemberPatch WithAllPrivacy(this MemberPatch member, PrivacyLevel level)
    {
        foreach (var subject in Enum.GetValues(typeof(MemberPrivacySubject)))
            member.WithPrivacy((MemberPrivacySubject)subject, level);
        return member;
    }

    public static bool TryParseMemberPrivacy(string input, out MemberPrivacySubject subject)
    {
        switch (input.ToLowerInvariant())
        {
            case "name":
                subject = MemberPrivacySubject.Name;
                break;
            case "description":
            case "desc":
            case "describe":
            case "d":
            case "bio":
            case "info":
            case "text":
            case "intro":
                subject = MemberPrivacySubject.Description;
                break;
            case "banner":
            case "b":
            case "splash":
            case "cover":
                subject = MemberPrivacySubject.Banner;
                break;
            case "avatar":
            case "pfp":
            case "pic":
            case "icon":
                subject = MemberPrivacySubject.Avatar;
                break;
            case "birthday":
            case "birth":
            case "bday":
            case "birthdate":
            case "bdate":
            case "cakeday":
            case "bd":
                subject = MemberPrivacySubject.Birthday;
                break;
            case "pronouns":
            case "pronoun":
            case "prns":
            case "pn":
                subject = MemberPrivacySubject.Pronouns;
                break;
            case "meta":
            case "metadata":
            case "created":
                subject = MemberPrivacySubject.Metadata;
                break;
            case "proxy":
            case "proxies":
            case "tag":
            case "tags":
                subject = MemberPrivacySubject.Proxy;
                break;
            case "visibility":
            case "hidden":
            case "shown":
            case "visible":
            case "list":
                subject = MemberPrivacySubject.Visibility;
                break;
            default:
                subject = MemberPrivacySubject.Name;
                return false;
        }

        return true;
    }
}