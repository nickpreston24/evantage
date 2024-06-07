using System.ComponentModel;
using CodeMechanic.Diagnostics;

namespace CodeMechanic.Airtable;

public static class ObjectToDictionaryHelper
{
    public static IDictionary<string, object> ToDictionary(this object source)
    {
        return source.ToDictionary<object>();
    }

    public static IDictionary<string, T> ToDictionary<T>(this object source)
    {
        if (source == null)
            ThrowExceptionWhenSourceArgumentIsNull();

        var dictionary = new Dictionary<string, T>();
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            AddPropertyToDictionary<T>(property, source, dictionary);

        return dictionary;
    }

    private static void AddPropertyToDictionary<T>(
        PropertyDescriptor property
        , object source,
        Dictionary<string, T> dictionary)
    {
        // Console.WriteLine("adding baby propery " + property.Name);
        object value = property.GetValue(source);
        var type = property.PropertyType;
        // property.Name.Dump("prop name ::>>");
        // value.Dump("prop value ::>> ");
        // Console.WriteLine("wtf is the type? " + type.Name);
        if (IsOfType<T>(value))
            dictionary.TryAdd(property.Name, (T)value);
        if (type == typeof(string))
            dictionary.TryAdd(property.Name, (T)value);
        // Console.WriteLine("total props: " + dictionary.Count);
    }

    private static bool IsOfType<T>(object value)
    {
        return value is T;
    }

    private static void ThrowExceptionWhenSourceArgumentIsNull()
    {
        throw new ArgumentNullException("source",
            "Unable to convert object to a dictionary. The source object is null.");
    }
}