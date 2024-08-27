using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace HsManCommonLibrary.Reflections.Finders;

class MemberFinderRegexDictionaryHolder
{
    public static readonly ConcurrentDictionary<string, Regex> RegexCache = new();
}