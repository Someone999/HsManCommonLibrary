using System.Reflection;

namespace HsManCommonLibrary.Reflections;

public class MemberFindOptions
{
    public BindingFlags? BindingFlags { get; set; }
    public string MemberName { get; set; } = "";

    public static MemberFindOptions Empty { get; } = new MemberFindOptions();
}