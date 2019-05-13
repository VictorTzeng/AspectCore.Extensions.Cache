using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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
    }
}