using System.Reflection;
using System.Text.RegularExpressions;

namespace HsManCommonLibrary.Reflections.Finders;

public class MemberFinder<TMember> where TMember : MemberInfo
{
    protected readonly Type InnerType;

    public MemberFinder(Type innerType)
    {
        InnerType = innerType;
    }

    public TMember? FindMember(string name, BindingFlags bindingFlags = BindingFlagsConstants.PublicMembers)
    {
        var members = InnerType.GetMember(name, bindingFlags).OfType<TMember>().ToArray();
        if (members.Length > 1)
        {
            throw new AmbiguousMatchException();
        }

        return members.Length == 0 ? null : members[0];
    }

    public TMember[] FindMembers(string name, BindingFlags bindingFlags = BindingFlagsConstants.PublicMembers)
    {
        return InnerType.GetMember(name, bindingFlags).OfType<TMember>().ToArray();
    }

    public TMember[] SearchMembersByRegex(string pattern, BindingFlags bindingFlags = BindingFlagsConstants.PublicMembers)
    {
        if (!MemberFinderRegexDictionaryHolder.RegexCache.TryGetValue(pattern, out var regex))
        {
            regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MemberFinderRegexDictionaryHolder.RegexCache.TryAdd(pattern, regex);
        }

        var members = InnerType.GetMembers(bindingFlags).OfType<TMember>().ToArray();
        var matchingMembers = members.Where(member => regex.IsMatch(member.Name)).ToArray();
        return matchingMembers;
    }

    public bool TryGetMember(string name, out TMember? memberInfo, BindingFlags bindingFlags = BindingFlagsConstants.PublicMembers)
    {
        memberInfo = FindMember(name, bindingFlags);
        return memberInfo != null;
    }
}