using System.Reflection;
using HarmonyLib;

namespace Rhynia.Baseline.Util;

/// <summary>
/// A quick approach to access instance's fields and properties using reflection.
/// </summary>
public static class ReflectionExtension
{
    private static readonly Dictionary<Type, Dictionary<string, FieldInfo>> _fieldCache = [];
    private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _propertyCache = [];

    public static T ReflectGetField<T>(this object obj, string fieldName)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));
        if (string.IsNullOrEmpty(fieldName))
            throw new ArgumentNullException(nameof(fieldName));

        var type = obj.GetType();
        var typeCache = _fieldCache.GetOrAddDefault(type);
        var field = typeCache.GetOrAdd(
            fieldName,
            () =>
                type.Field(fieldName)
                ?? throw new ArgumentException(
                    $"Field '{fieldName}' not found in type '{type.FullName}'."
                )
        );

        return (T)field.GetValue(obj);
    }

    public static void ReflectSetField<T>(this object obj, string fieldName, T value)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));
        if (string.IsNullOrEmpty(fieldName))
            throw new ArgumentNullException(nameof(fieldName));

        var type = obj.GetType();
        var typeCache = _fieldCache.GetOrAddDefault(type);
        var field = typeCache.GetOrAdd(
            fieldName,
            () =>
                type.Field(fieldName)
                ?? throw new ArgumentException(
                    $"Field '{fieldName}' not found in type '{type.FullName}'."
                )
        );

        field.SetValue(obj, value);
    }

    public static T ReflectGetProperty<T>(this object obj, string propertyName)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));
        if (string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName));

        var type = obj.GetType();
        var typeCache = _propertyCache.GetOrAddDefault(type);
        var property = typeCache.GetOrAdd(
            propertyName,
            () =>
                type.Property(propertyName)
                ?? throw new ArgumentException(
                    $"Property '{propertyName}' not found in type '{type.FullName}'."
                )
        );

        return (T)property.GetValue(obj);
    }

    public static void ReflectSetProperty<T>(this object obj, string propertyName, T value)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));
        if (string.IsNullOrEmpty(propertyName))
            throw new ArgumentNullException(nameof(propertyName));

        var type = obj.GetType();
        var typeCache = _propertyCache.GetOrAddDefault(type);
        var property = typeCache.GetOrAdd(
            propertyName,
            () =>
                type.Property(propertyName)
                ?? throw new ArgumentException(
                    $"Property '{propertyName}' not found in type '{type.FullName}'."
                )
        );

        property.SetValue(obj, value);
    }
}
