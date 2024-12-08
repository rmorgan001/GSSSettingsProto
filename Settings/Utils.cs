using System.ComponentModel;

namespace Settings
{
    internal class Utils
    {

        public static T ConvertString<T>(string value)
        {
            var targetType = typeof(T);
            var converter = TypeDescriptor.GetConverter(targetType);
            if(!converter.CanConvertFrom(typeof(string))) throw new NotSupportedException($"Converter not found for {targetType}");
            return (T)converter.ConvertFromString(value); //converting null to a non-null type could be an issue
        }
    }
}
