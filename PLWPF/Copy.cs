using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PLWPF
{
    public static class Copy
    {
        public static T GetCopy<T>(this T source)
        {
            var isNotSerializable = !typeof(T).IsSerializable;

            if (isNotSerializable)
                throw new ArgumentException("The type must be serializable.", "source");

            var sourceIsNull = ReferenceEquals(source, null);

            if (sourceIsNull)
                return default(T);

            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
