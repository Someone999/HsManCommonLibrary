using System.Reflection;

namespace HsManCommonLibrary.Reflections;

public static class BindingFlagsConstants
{
    public const BindingFlags PublicMembers = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
    public const BindingFlags StaticMembers = BindingFlags.Public | BindingFlags.Static;
    public const BindingFlags AllMembers = PublicMembers | BindingFlags.NonPublic;
    public const BindingFlags NonPublicInstanceMembers = BindingFlags.NonPublic | BindingFlags.Instance;
    public const BindingFlags NonPublicStaticMembers = BindingFlags.NonPublic | BindingFlags.Static;
    public const BindingFlags AllInstanceMembers = PublicMembers | BindingFlags.NonPublic | BindingFlags.Instance;
    public const BindingFlags AllStaticMembers = PublicMembers | BindingFlags.NonPublic | BindingFlags.Static;
    public const BindingFlags ProtectedMembers = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
    public const BindingFlags InheritedMembers = BindingFlags.Public | ProtectedMembers;
    public const BindingFlags PublicStaticMembersOnly = BindingFlags.Public | BindingFlags.Static;
    public const BindingFlags AllNonPublicMembers = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
}