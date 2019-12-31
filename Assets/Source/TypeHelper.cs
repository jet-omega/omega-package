using System;
using System.Collections.Generic;
using System.Reflection;

namespace Omega.Package
{
    public class TypeHelper
    {
        private static Dictionary<Type, TypeHelper> _heleprs = new Dictionary<Type, TypeHelper>();

        public static TypeHelper For(Type type)
        {
            if(type is null)
                throw new ArgumentNullException(nameof(type));

            return ForInternal(type);
        }

        public static TypeHelper For<T>() => TypeHelperBinder<T>.Helper;

        private Type _type;
        private (FieldInfo field, BindingFlags flags)[] _fields;
        private Dictionary<Type, TypeHelper> _generics;

        public Type Type => _type;

        public void GetFields(List<FieldInfo> fields, BindingFlags bindingFlags)
        {
            if (_fields is null)
                CacheFields();

            for (int i = 0; i < _fields.Length; i++)
            {
                var fieldEntry = _fields[i];
                if ((fieldEntry.flags & bindingFlags) == fieldEntry.flags)
                    fields.Add(fieldEntry.field);
            }
        }

        [Obsolete("Use InstanceFactory")]
        public object CreateInstance() => InstanceFactory.Create(_type);

        public FieldInfo[] GetFields(BindingFlags bindingFlags)
        {
            var list = ListPool<FieldInfo>.Rent();
            GetFields(list, bindingFlags);
            return ListPool<FieldInfo>.ReturnToArray(list);
        }

        private TypeHelper(Type type)
        {
            _type = type;
            if (type.IsGenericTypeDefinition)
                _generics = new Dictionary<Type, TypeHelper>(3);
        }

        [Obsolete("Use ForGenericType method")]
        public TypeHelper GetGenericType(Type type)
            => ForGenericType(type);

        public TypeHelper ForGenericType<T>()
            => ForGenericType(typeof(T));
        public TypeHelper ForGenericType(params Type[] genericArguments)
        {
            if (_generics is null)
                throw new InvalidOperationException(nameof(genericArguments));
            if (genericArguments.Length is 0)
                throw new ArgumentException($"{genericArguments} can not be empty");

            TypeHelper typeHelperForGenericType;

            if (genericArguments.Length is 1)
            {
                if (!_generics.TryGetValue(genericArguments[0], out typeHelperForGenericType))
                {
                    typeHelperForGenericType = GetTypeHelperForGenericArgs(genericArguments);
                    _generics.Add(genericArguments[0], typeHelperForGenericType);
                }
            }
            else
                typeHelperForGenericType = GetTypeHelperForGenericArgs(genericArguments);

            return typeHelperForGenericType;
        }
        private TypeHelper GetTypeHelperForGenericArgs(Type[] genericArgs)
        {
            var genericType = _type.MakeGenericType(genericArgs);
            return For(genericType);
        }

        private void CacheFields()
        {
            var fields = _type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static |
                                         BindingFlags.NonPublic);

            _fields = new (FieldInfo field, BindingFlags flags)[fields.Length];
            for (int i = 0; i < _fields.Length; i++)
            {
                var field = fields[i];
                var flag = default(BindingFlags);

                flag |= field.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
                flag |= field.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;

                _fields[i] = (field, flag);
            }
        }
        
        internal static TypeHelper ForInternal(Type type)
        {
            if (!_heleprs.TryGetValue(type, out var typeOfNode))
            {
                typeOfNode = new TypeHelper(type);
                _heleprs.Add(type, typeOfNode);
            }

            return typeOfNode;
        }
        
        public static implicit operator Type(TypeHelper typeHelper)
        {
            return typeHelper?.Type;
        }
        
        /// <summary>
        /// Вспомагательный класс, для быстрого связываения типа T его TypeHelper`ом 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static class TypeHelperBinder<T>
        {
            public static readonly TypeHelper Helper;

            static TypeHelperBinder()
            {
                var type = typeof(T);
                var helper = ForInternal(type);
                Helper = helper;
            }
        }
    }
}