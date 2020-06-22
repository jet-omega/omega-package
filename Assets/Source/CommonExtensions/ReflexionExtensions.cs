﻿using System;
using System.Reflection;

namespace Omega.Package
{
    public static class ReflexionExtensions
    {
        public static Type GetReturnType(this MemberInfo memberInfo)
        {
            switch (memberInfo)
            {
                case FieldInfo fieldInfo:
                    return fieldInfo.FieldType;
                case PropertyInfo propertyInfo:
                    return propertyInfo.PropertyType;
                case MethodInfo methodInfo:
                    return methodInfo.ReturnType;
                case EventInfo eventInfo:
                    return eventInfo.EventHandlerType;
                default:
                    return null;
            }
        }

        public static object GetMemberValue(this MemberInfo member, object obj)
        {
            switch (member)
            {
                case FieldInfo info:
                    return info.GetValue(obj);
                case PropertyInfo info:
                    return info.GetGetMethod(true).Invoke(obj, null);
                default:
                    throw new ArgumentException($"Can't get the value of a {member.GetType().Name}");
            }
        }

        public static void SetMemberValue(this MemberInfo member, object obj, object value)
        {
            switch (member)
            {
                case FieldInfo info:
                    info.SetValue(obj, value);
                    break;
                case PropertyInfo info:
                    var setMethod = info.GetSetMethod(true);
                    if (setMethod == null)
                        throw new ArgumentException($"Property {info.Name} has no setter");
                    setMethod.Invoke(obj, new[]{ value });
                    break;
                default:
                    throw new ArgumentException($"Can't set the value of a {member.GetType().Name}");
            }
        }
    }
}
