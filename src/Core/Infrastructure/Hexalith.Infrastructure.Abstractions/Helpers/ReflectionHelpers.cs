namespace Hexalith.Infrastructure.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;

    using Microsoft.Extensions.Primitives;

    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets the concrete classes.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Type[].</returns>
        public static Type[] GetConcreteClasses(this Type type, Assembly assembly)
        {
            return GetInterfaceConcreteClassTypes(assembly, type);
        }

        /// <summary>
        /// Gets the concrete classes.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>Type[].</returns>
        /// <exception cref="ArgumentNullException">assemblies</exception>
        /// <exception cref="System.ArgumentNullException">assemblies</exception>
        public static Type[] GetConcreteClasses(this Type type, IEnumerable<Assembly> assemblies)
        {
            _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

            return assemblies.SelectMany(p => GetInterfaceConcreteClassTypes(p, type)).ToArray();
        }

        /// <summary>
        /// Gets the concrete classes.
        /// </summary>
        /// <typeparam name="TInterface">The type of the t interface.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Type[].</returns>
        public static Type[] GetConcreteClasses<TInterface>(this Assembly assembly)
        {
            return GetInterfaceConcreteClassTypes<TInterface>(assembly);
        }

        /// <summary>
        /// Gets the interface concrete class types.
        /// </summary>
        /// <typeparam name="TInterface">The type of the t interface.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns>System.Type[].</returns>
        public static Type[] GetInterfaceConcreteClassTypes<TInterface>(Assembly assembly)
        {
            return GetInterfaceConcreteClassTypes(assembly, typeof(TInterface));
        }

        /// <summary>
        /// Gets the interface concrete class types.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns>System.Type[].</returns>
        /// <exception cref="ArgumentNullException">assembly</exception>
        /// <exception cref="ArgumentNullException">interfaceType</exception>
        /// <exception cref="ArgumentException">
        /// The type {interfaceType.Name} is not an interface. - interfaceType
        /// </exception>
        /// <exception cref="System.ArgumentNullException">assembly</exception>
        /// <exception cref="System.ArgumentNullException">interfaceType</exception>
        /// <exception cref="System.ArgumentException">
        /// The type {interfaceType.Name} is not an interface. - interfaceType
        /// </exception>
        /// <exception cref="System.ArgumentNullException">assembly</exception>
        /// <exception cref="ArgumentNullException">interfaceType</exception>
        public static Type[] GetInterfaceConcreteClassTypes(Assembly assembly, Type interfaceType)
        {
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _ = interfaceType ?? throw new ArgumentNullException(nameof(interfaceType));

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException($"The type {interfaceType.Name} is not an interface.", nameof(interfaceType));
            }
            return assembly.GetTypes()
                        .Where(p => p.IsClass && p.HasInterface(interfaceType))
                        .ToArray();
        }

        /// <summary>
        /// Gets the interface generic arguments.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericInterfaceType">Type of the generic interface.</param>
        /// <returns>Type[].</returns>
        /// <exception cref="ArgumentNullException">type</exception>
        /// <exception cref="ArgumentNullException">genericInterfaceType</exception>
        /// <exception cref="ArgumentException">
        /// The type {genericInterfaceType.Name} is not a generic interface. - genericInterfaceType
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The type {genericInterfaceType.Name} is not assignable from {type.Name}. - genericInterfaceType
        /// </exception>
        /// <exception cref="System.ArgumentNullException">type</exception>
        /// <exception cref="System.ArgumentNullException">genericInterfaceType</exception>
        /// <exception cref="System.ArgumentException">
        /// The type {genericInterfaceType.Name} is not a generic interface. - genericInterfaceType
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The type {genericInterfaceType.Name} is not assignable from {type.Name}. - genericInterfaceType
        /// </exception>
        public static Type[] GetInterfaceGenericArguments(this Type type, Type genericInterfaceType)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));
            _ = genericInterfaceType ?? throw new ArgumentNullException(nameof(genericInterfaceType));
            if (!genericInterfaceType.IsInterface || !genericInterfaceType.IsGenericType)
            {
                throw new ArgumentException($"The type {genericInterfaceType.Name} is not a generic interface.", nameof(genericInterfaceType));
            }
            foreach (Type t in type.GetInterfaces())
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == genericInterfaceType)
                {
                    return t.GetGenericArguments();
                }
            }
            throw new ArgumentException($"The type {genericInterfaceType.Name} is not assignable from {type.Name}.", nameof(genericInterfaceType));
        }

        public static IDictionary<string, object> GetPropertyNotNullValues(this object obj)
            => GetPropertyValues(obj)
                    .Where(p => p.Value != null)
                    .ToDictionary(k => k.Key, v => v.Value ?? throw new ArgumentException("only to remove nullability check error", nameof(obj)));

        public static IEnumerable<KeyValuePair<string, object?>> GetPropertyValues(this object obj) => obj
            .GetType()
            .GetProperties()
            .Where(x => x.CanRead)
            .Select(x => new KeyValuePair<string, object?>(x.Name, x.GetValue(obj, null)));

        /// <summary>
        /// Determines whether the specified interface type has interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns><c>true</c> if the specified interface type has interface; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">type</exception>
        /// <exception cref="ArgumentNullException">interfaceType</exception>
        /// <exception cref="ArgumentException">
        /// The type {interfaceType.Name} is not an interface. - interfaceType
        /// </exception>
        /// <autogeneratedoc/>
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));
            _ = interfaceType ?? throw new ArgumentNullException(nameof(interfaceType));

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException($"The type {interfaceType.Name} is not an interface.", nameof(interfaceType));
            }

            if (interfaceType.IsAssignableFrom(type))
            {
                return true;
            }

            if (interfaceType.IsGenericType && type
                                                    .GetInterfaces()
                                                    .Any(p => p.IsGenericType && p.GetGenericTypeDefinition() == interfaceType))
            {
                return true;
            }

            return false;
        }

        public static ExpandoObject ToDynamic(this IDictionary<string, object?> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDict = (IDictionary<string, object?>)expando;

            var dict = dictionary.Select(
                p => new KeyValuePair<string, object?>(
                    p.Key,
                    p.Value switch
                    {
                        IDictionary<string, object?> dict
                            => dict.ToDynamic(),
                        ICollection col
                            => getCollection(col),
                        _ => p.Value
                    }));
            foreach (var pair in dict)
            {
                expandoDict.Add(pair);
            }
            return expando;

            static object? getCollection(ICollection collection)
            {
                List<object?> list = new();
                foreach (var item in collection)
                {
                    list.Add(item switch { IDictionary<string, object?> subDict => subDict.ToDynamic(), _ => item });
                }
                return list;
            }
        }

        public static object ToObject(this IEnumerable<KeyValuePair<string, StringValues>> values, Type type)
        {
            object? obj = Activator.CreateInstance(type);
            if (obj == null)
            {
                throw new TypeInitializationException(type.FullName, null);
            }

            foreach (var pair in values)
            {
                if (!StringValues.IsNullOrEmpty(pair.Value))
                {
                    obj.SetValue(pair.Key, pair.Value);
                }
            }

            return obj;
        }

        private static void SetValue(this object obj, string propertyName, string jsonValue)
        {
            BindingFlags bindingFlags =
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.IgnoreCase |
                BindingFlags.SetProperty |
                BindingFlags.SetField |
                BindingFlags.FlattenHierarchy;

            Type? type = obj.GetType() ?? throw new TypeLoadException("The object type can't be retreived.");
            while (type != null)
            {
                PropertyInfo? property = type.GetProperty(propertyName, bindingFlags);
                if (property != null)
                {
                    var setMethod = property.GetSetMethod(true);
                    if (setMethod != null)
                    {
                        setMethod.Invoke(obj, new[] { JsonSerializer.Deserialize(jsonValue, property.PropertyType) });
                        return;
                    }
                }

                FieldInfo? field = type.GetField(propertyName, bindingFlags);
                if (field == null)
                {
                    // Property setters do not exist for automatic readonly properties. Try to set
                    // the value of the autogenerated underlying field.
                    field = type.GetField($"<{propertyName}>k__BackingField", bindingFlags);
                }

                if (field != null)
                {
                    field.SetValue(obj, JsonSerializer.Deserialize(jsonValue, field.FieldType));
                    return;
                }

                type = type.BaseType;
            }

            throw new KeyNotFoundException($"The property with name '{propertyName}' not found on object type '{obj.GetType().Name}'.");
        }
    }
}