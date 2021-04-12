using System;
using System.Reflection;

namespace Omega.Package
{
    /// <summary>
    /// Based on Sirenix.Utilities - https://odininspector.com/
    /// </summary>
    public static class ReflectionExtensions
    {
        public static Type GetReturnType(this MemberInfo memberInfo)
        {
            if (memberInfo is null)
                throw new NullReferenceException(nameof(memberInfo));

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
                    throw new InvalidOperationException($"not support member type:{memberInfo.GetType()}");
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
                    throw new ArgumentException($"can't get the value of a {member.GetType().Name}");
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
                        throw new ArgumentException($"property {info.Name} has no setter");
                    setMethod.Invoke(obj, new[] {value});
                    break;
                default:
                    throw new ArgumentException($"can't set the value of a {member.GetType().Name}");
            }
        }
    }
}