using System.Globalization;
using System.Reflection;

namespace evantage.Services;

public static class DynamicExtensions
{
    public static object SetPropertyValue(this object entity, PropertyInfo prop, object value)
    {
        if (prop == null) return entity;
        if (value == null)
        {
            prop.SetValue(entity, null, null);
            return entity;
        }

        if (prop.PropertyType == typeof(string))
        {
            prop.SetValue(entity, value.ToString().Trim(), null);
        }
        else if (prop.PropertyType == typeof(char))
        {
            prop.SetValue(entity, value.ToString()[0], null);
        }
        else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
        {
            prop.SetValue(entity, ParseBoolean(value.ToString()), null);
        }
        else if (prop.PropertyType == typeof(long))
        {
            prop.SetValue(entity, long.Parse(value.ToString()), null);
        }
        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
        {
            prop.SetValue(entity, int.Parse(value.ToString()), null);
        }
        else if (prop.PropertyType == typeof(decimal))
        {
            prop.SetValue(entity, decimal.Parse(value.ToString()), null);
        }
        else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
        {
            double number;
            bool isValid = double.TryParse(value.ToString(), out number);
            if (isValid)
            {
                prop.SetValue(entity, double.Parse(value.ToString()), null);
            }
        }
        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
        {
            DateTime date;
            bool isValid = DateTime.TryParse(value.ToString(), out date);
            if (isValid)
            {
                prop.SetValue(entity, date, null);
            }
            else
            {
                isValid = DateTime.TryParseExact(value.ToString(), "MMddyyyy", new CultureInfo("en-US"),
                    DateTimeStyles.AssumeLocal, out date);
                if (isValid)
                {
                    prop.SetValue(entity, date, null);
                }
            }
        }
        else if (prop.PropertyType == typeof(Guid))
        {
            Guid guid;
            bool isValid = Guid.TryParse(value.ToString(), out guid);
            if (isValid)
            {
                prop.SetValue(entity, guid, null);
            }
            else
            {
                isValid = Guid.TryParseExact(value.ToString(), "B", out guid);
                if (isValid)
                {
                    prop.SetValue(entity, guid, null);
                }
            }
        }

        return entity;
    }

    public static bool ParseBoolean(object value)
    {
        if (value == null || value == DBNull.Value) return false;

        switch (value.ToString().ToLowerInvariant())
        {
            case "1":
            case "y":
            case "yes":
            case "true":
                return true;

            case "0":
            case "n":
            case "no":
            case "false":
            default:
                return false;
        }
    }

    public static T ToStatic<T>(object expando)
    {
        var entity = Activator.CreateInstance<T>();

        //ExpandoObject implements dictionary
        var dict = expando as IDictionary<string, object>;
        var props = typeof(T).GetProperties();
        // properties.Select(kvp => kvp.Key).ToArray().Dump("all keys");

        if (dict == null)
            return entity;

        foreach (var propertyInfo in props)
        {
            var hasvalue = dict.TryGetValue(propertyInfo.Name, out object entry_value);
            Console.WriteLine("key: " + propertyInfo.Name + "value: " + entry_value);
            entity.SetPropertyValue(propertyInfo, entry_value);

            // var value = CreateSafeValue(entry_value, propertyInfo);
            // Console.WriteLine($"{propertyInfo.Name}: {value}");
            // if (propertyInfo != null && hasvalue)
            // propertyInfo.SetValue(entity, value, null);
            // SetPropertyValue(entity, value, propertyInfo);
        }


        return entity;
    }

//     /// <summary>
//     /// PropertyCache stores the properties we wish to use again so we only have to run Reflection once per property.
//     /// </summary>
//     private static readonly ConcurrentDictionary<Type, ICollection<PropertyInfo>> _propertyCache =
//         new ConcurrentDictionary<Type, ICollection<PropertyInfo>>();
//
//     public static T MapTo<T>(
//         dynamic record
//         , List<PropertyInfo> props = null
//     )
//         where T : class, new()
//     {
//         var type = typeof(T);
//
//         // if no props passed as an override, get them from the cache
//         var properties = props.Count > 0
//             ? props
//             : _propertyCache.TryGetProperties<T>(true);
//
//
//         if (properties.Count == 0) return new T();
//
//         var obj = new T();
//
//         foreach (var prop in properties ?? Enumerable.Empty<PropertyInfo>())
//         {
//             string name = prop.Name /*.Dump("key")*/;
//             // var value = prop.GetValue(obj);
//             var value = record[name];
//
//             var next_value = CreateSafeValue(value, prop);
//
//             prop.SetValue(obj, next_value /*.Dump("value")*/, null);
//         }
//
//         return obj;
//     }
//
    private static object CreateSafeValue(object value, PropertyInfo prop)
    {
        Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

        object safeValue =
            value == null
                ? null
                : Convert.ChangeType(value, propType);

        return safeValue;
    }
}