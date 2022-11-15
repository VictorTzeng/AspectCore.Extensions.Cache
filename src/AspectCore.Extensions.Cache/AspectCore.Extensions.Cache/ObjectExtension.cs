using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#if NETSTANDARD2_0_OR_GREATER
using System.Runtime.Serialization.Formatters.Binary;
#endif
#if NET5_0_OR_GREATER
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
#endif
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace AspectCore.Extensions.Cache
{
    public static class ObjectExtension
    {
        public static bool IsTask(this Type source)
        {
            return source.BaseType == typeof(Task);
        }

        public static byte[] ToBytes(this object obj)
        {
            if (obj == null)
                return null;
#if NETSTANDARD2_0_OR_GREATER
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
#endif
#if NET5_0_OR_GREATER
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj, GetJsonSerializerOptions()));
#endif
        }

        public static object ToObject(this byte[] source)
        {
            if (source == null || !source.Any())
                return default;
#if NETSTANDARD2_0_OR_GREATER
            using (var memStream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                memStream.Write(source, 0, source.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = bf.Deserialize(memStream);
                return obj;
            }
#endif
#if NET5_0_OR_GREATER
            return JsonSerializer.Deserialize<object>(source, GetJsonSerializerOptions());
#endif
        }

#if NET5_0_OR_GREATER
        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
#endif
    }
}