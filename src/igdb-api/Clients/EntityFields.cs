using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace igdb_api.Clients;


public class EntityFields
{
    private static IEnumerable<string>? GetFieldsFrom(Type type)
    {
        var method = (from x in typeof(EntityFields).GetMethods(BindingFlags.Static | BindingFlags.Public)
            where x.Name == "GetFieldsFrom" &&
                  x.IsGenericMethod
            select x).SingleOrDefault();

        var genericMethod = method?.MakeGenericMethod(type);

        return genericMethod?.Invoke(null, null) as IEnumerable<string>;
    }

    public static string? GetApiExpressionFromPredicate<TResult>(Expression<Func<TResult, bool>> predicate)
    {
        var type = predicate.Parameters.First().Type;

        switch (predicate.Body.NodeType)
        {
            case ExpressionType.Call:
            {
                if (predicate.Body is not MethodCallExpression methodCall) return null;

                switch (methodCall.Method.Name)
                {
                    case "Contains":
                    {
                        var values = Expression.Lambda(((methodCall.Arguments.First() as MemberExpression)!))
                            .Compile().DynamicInvoke();

                        var name = (methodCall.Arguments.Last() as MemberExpression)?.Member.Name;

                        if (name is null)
                            return null;

                        if (GetPropertyFromMemberName(type, name, out var property))
                            return null;

                        var lhs = GetFieldNameFromProperty(property!)?.SingleOrDefault();

                        if (values is int[] numerics)
                        {
                            return $"{lhs}=({string.Join(',', numerics)})";
                        }
                    }
                        break;
                }

                return "";
            }
            default:
            case ExpressionType.Equal:
            {
                var expression = (BinaryExpression)predicate.Body;

                var left = (MemberExpression)expression.Left;

                var name = left.Member.Name;

                if (!GetPropertyFromMemberName(type, name, out var property))
                    return null;

                var lhs = GetFieldNameFromProperty(property!)?.SingleOrDefault();
                object? value = null;

                if (expression.Right is ConstantExpression right)
                {
                    value = right;
                }
                else
                {
                    var field = (MemberExpression)expression.Right;

                    var objectMember = Expression.Convert(field, typeof(object));
                    var getter = Expression.Lambda<Func<object>>(objectMember);

                    value = getter.Compile()();
                }

                return property!.PropertyType == typeof(string) ? $"{lhs} = '{value}'" : $"{lhs} = {value}";
            }
        }
    }

    private static bool GetPropertyFromMemberName(Type type, string name, out PropertyInfo? property)
    {
        property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .SingleOrDefault(p => p.Name == name);

        return property is not null;
    }

    public static IEnumerable<string> GetFieldsFrom<TEntity>() where TEntity : class
    {
        var properties = from p in typeof(TEntity)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            select p;

        var result = new List<string>();

        foreach (var property in properties)
        {
            var fields = GetFieldNameFromProperty(property);

            if (fields is not null)
                result.AddRange(fields);
        }

        return result;
    }

    private static IEnumerable<string>? GetFieldNameFromProperty(PropertyInfo property)
    {
        var jsonProperty = property.GetCustomAttribute<JsonPropertyNameAttribute>();
        var propertyName = (jsonProperty != null ? jsonProperty.Name : property.Name).ToLower();
        var referenceProperty = property.GetCustomAttribute<ReferenceAttribute>();

        if (property.PropertyType.IsTypeDefinition && referenceProperty is null)
        {
            return new[] { propertyName };
        }

        if (!property.PropertyType.IsGenericType)
        {
            if (referenceProperty is null) return null;

            var fields = GetFieldsFrom(referenceProperty.Type);

            return from f in fields select $"{propertyName}.{f}";
        }
        else
        {
            if (property.PropertyType.GenericTypeArguments.SingleOrDefault(s => s.IsClass) == null)
            {
                return [propertyName];
            }
                
            var fields = GetFieldsFrom(property.PropertyType.GenericTypeArguments[0]);

            return (from f in fields select $"{propertyName}.{f}");

        }
    }
}