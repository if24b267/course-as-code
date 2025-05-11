using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class CustomDateTimeConverter : IYamlTypeConverter
{
    private readonly string[] _formats;
    private readonly IFormatProvider _provider;

    public CustomDateTimeConverter(IFormatProvider provider, string[] formats)
    {
        _provider = provider;
        _formats = formats;
    }

    public bool Accepts(Type type)
    {
        return type == typeof(DateTime) || type == typeof(DateTime?);
    }

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer nestedObjectDeserializer)
    {
        var scalar = parser.Consume<Scalar>();

        if (string.IsNullOrWhiteSpace(scalar.Value))
        {
            if (type == typeof(DateTime?))
                return null;

            throw new YamlException(scalar.Start, scalar.End, "Empty string is not allowed for non-nullable DateTime.");
        }

        if (DateTime.TryParseExact(scalar.Value, _formats, _provider, DateTimeStyles.None, out var parsed))
        {
            return parsed;
        }

        throw new YamlException(scalar.Start, scalar.End,
            $"Invalid DateTime format: '{scalar.Value}'. Expected format: {string.Join(" or ", _formats)}");
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is DateTime dt)
        {
            emitter.Emit(new Scalar(dt.ToString(_formats[0], _provider)));
        }
        else
        {
            emitter.Emit(new Scalar(string.Empty));
        }
    }
}
