using System;
using System.Reflection;

namespace OrvilleX.Dapper
{
    internal static class TypeExtensions
    {
        public static string Name(this Type type)
        {
            return type.Name;
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }
        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }
        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        public static TypeCode GetTypeCode(Type type)
        {
            return Type.GetTypeCode(type);
        }

        public static MethodInfo GetPublicInstanceMethod(this Type type, string name, Type[] types)
        {
            var method = type.GetMethod(name, types);
            return (method != null && method.IsPublic && !method.IsStatic) ? method : null;
        }
    }
}
