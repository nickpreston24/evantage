using System.Reflection;

namespace CodeMechanic.Types;

public static class TypeExtensions
{
    public static T OrNullClass<T>(this T instance)
    {
        return Activator.CreateInstance<T>().WithDefaultProperties();
    }

    public static T WithDefaultProperties<T>(
        this T instance
        , Func<T, T> transform = null)
    {
        // get props
        var props = typeof(T).GetProperties();

        // set default values
        foreach (var prop in props)
        {
            var safe_value = CreateSafeValue(prop, instance);
            prop.SetValue(instance, safe_value);
        }

        // then set the values the dev actually wanted as defaults.
        var updated_instance = instance.Map(transform);
        return updated_instance;
    }

    private static object CreateSafeValue(PropertyInfo prop, object property_value)
    {
        // Ignore DBNull and return null.
        // Here's why: https://itecnote.com/tecnote/r-the-point-of-dbnull/
        if (Convert.IsDBNull(property_value))
        {
            return null;
        }

        // This line produces a casting bug in mscorlib because it does NOT handle DBNull.
        // DO NOT REMOVE the above code!!!
        Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

        object safeValue =
            property_value == null
                ? null
                : Convert.ChangeType(property_value, propType);

        return safeValue;
    }
}