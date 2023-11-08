namespace HsManCommonLibrary.Logger.Serializers;

public class SerializerMap
{
    private readonly Dictionary<Type, IObjectSerializer> _serializers = new Dictionary<Type, IObjectSerializer>();

    public void Register<T>(IObjectSerializer<T> serializer)
    {
        var elementType = typeof(T);
        if (_serializers.ContainsKey(elementType))
        {
            throw new InvalidOperationException("Serializer is existed. Use Replace to replace existed one to yours.");
        }
        
        _serializers.Add(elementType, serializer);
    }

    public IObjectSerializer Replace<T>(IObjectSerializer serializer)
    {
        var elementType = typeof(T);
        if (_serializers.ContainsKey(elementType))
        {
            throw new InvalidOperationException("Serializer is not existed. Use Register to add yours.");
        }

        var oldSerializer = _serializers[elementType];
        _serializers[elementType] = serializer;
        return oldSerializer;
    }
}