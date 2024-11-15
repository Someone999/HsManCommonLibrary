namespace HsManCommonLibrary.Common;

public interface ICloneable<out T> : ICloneable
{
    new T Clone();
}