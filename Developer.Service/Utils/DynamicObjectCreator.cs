using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Developer.Service.Utils;

public static class DynamicObjectCreator
{
    private static readonly ConcurrentDictionary<Type, Func<object>> ParameterlessCtorDelegateCache = new();

    public static T CreateInstance<T>(Type instanceType) where T : class
    {
        if (!typeof(T).IsAssignableFrom(instanceType))
            throw new ArgumentException($"The type {typeof(T).Name} cannot be implemented by type {instanceType.Name}");

        var ctorDelegate = ParameterlessCtorDelegateCache.GetOrAdd(instanceType, GenerateCtorDelegate);

        Func<object> GenerateCtorDelegate(Type type)
        {
            ConstructorInfo? constructorInfo = type.GetConstructor(Type.EmptyTypes);

            if(constructorInfo == null)
            {
                throw new ArgumentException(
                    $"The type {typeof(T).Namespace} does not contain a parameterless constructor.");
            }

            var ctorNewExpr = Expression.New(constructorInfo);

            return Expression.Lambda<Func<object>>(ctorNewExpr).Compile();
        }

        return (T)ctorDelegate();
    }

    // NOTE: These functions can be utilized in future versions if desired to perform delegate caching for implementation
    // factories with up to one parameter to replicate how ASP.Net Core dependency injection system implementation
    // factories operate.
    //public static T CreateInstance<T>(Expression<Func<T>> ctorExpr) =>
    //    ctorExpr.Compile().Invoke();

    //public static T CreateInstance<TArg, T>(Expression<Func<TArg, T>> ctorExpr)
    //{
    //    var parameter = ctorExpr.Parameters[0];

    //    var argValue = GetArgumentValue<TArg>(parameter);

    //    return ctorExpr.Compile().Invoke(argValue);
    //}

    //private static TArg GetArgumentValue<TArg>(ParameterExpression parameterExpr) => default!;
}
