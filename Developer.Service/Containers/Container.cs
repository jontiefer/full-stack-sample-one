using System;
using System.Collections.Generic;
using System.Linq;
using Developer.Service.Utils;

namespace Developer.Service.Containers;

public enum ServiceLifetime
{
    Transient,
    Singleton
}

public struct ContainerImplementationData
{
    public ContainerImplementationData(Type implementationType, ServiceLifetime lifetime)
    {
        ImplementationType = implementationType;
        ImplementationFactory = null;
        Lifetime = lifetime;
    }

    public ContainerImplementationData(Delegate implementationFactory, ServiceLifetime lifetime)
    {
        ImplementationType = default;
        ImplementationFactory = implementationFactory;
        Lifetime = lifetime;
    }

    public Type? ImplementationType { get; set; }

    public Delegate? ImplementationFactory { get; set; }

    public ServiceLifetime Lifetime { get; set; }
}

/// <summary>
/// A container class that is designed to operate similar to the dependency injection container in ASP.Net Core.
/// </summary>
public class Container
{
    private readonly Dictionary<Type, List<ContainerImplementationData>> _dependencies = new();

    private readonly Dictionary<Type, List<object>> _singletonServices = new();

    public void Bind<TService, TImplementation>(ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService, new()
    {
        try
        {
            Type serviceType = typeof(TService);
            Type implementationType = typeof(TImplementation);

            var singleton = lifetime == ServiceLifetime.Singleton ?
                CreateService<TService>(implementationType) : default;

            if (!_dependencies.ContainsKey(serviceType))
            {
                _dependencies.Add(serviceType, new List<ContainerImplementationData>(
                    new[] { new ContainerImplementationData(implementationType, lifetime) }));

                if(lifetime ==  ServiceLifetime.Singleton)
                    _singletonServices.Add(serviceType, new List<object>(new [] { (object)singleton }));
            }
            else
            {
                var implementationList = _dependencies[serviceType];
                implementationList!.Add(new ContainerImplementationData(implementationType, lifetime));

                if (lifetime == ServiceLifetime.Singleton)
                {
                    var singletonList = _singletonServices[serviceType];
                    singletonList!.Add(singleton);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error attempting to bind dependency contract in container.");
            throw;
        }
    }

    public void Bind<TService>(Func<TService> implementationFactory, ServiceLifetime lifetime)
        where TService : class
    {
        try
        {
            Type interfaceType = typeof(TService);

            var singleton = lifetime == ServiceLifetime.Singleton ?
                implementationFactory() : default;


            if (!_dependencies.ContainsKey(interfaceType))
            {
                _dependencies.Add(interfaceType, new List<ContainerImplementationData>(
                    new[] { new ContainerImplementationData(implementationFactory, lifetime) }));

                if (lifetime == ServiceLifetime.Singleton)
                    _singletonServices.Add(interfaceType, new List<object>(new[] { singleton! }));
            }
            else
            {
                var implementationList = _dependencies[interfaceType];
                implementationList!.Add(new ContainerImplementationData(implementationFactory, lifetime));

                if (lifetime == ServiceLifetime.Singleton)
                {
                    var singletonList = _singletonServices[interfaceType];
                    singletonList!.Add(singleton!);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error attempting to bind dependency contract in container.");
            throw;
        }
    }

    public T GetService<T>() where T : class
    {
        if (!_dependencies.TryGetValue(typeof(T), out var implementations))
        {
            throw new KeyNotFoundException($"The type {typeof(T)} was not found to have a registered dependency.");
        }

        var dependencyImpl = implementations[^1];

        if (dependencyImpl.Lifetime == ServiceLifetime.Singleton)
        {
            return (T)_singletonServices[typeof(T)][^1];
        }

        return dependencyImpl.ImplementationType != null
            ? CreateService<T>(dependencyImpl.ImplementationType)
            : ((Func<T>)dependencyImpl.ImplementationFactory!)();
    }

    public IEnumerable<T> GetServices<T>() where T : class
    {
        
        if (!_dependencies.TryGetValue(typeof(T), out var implementations))
        {
            throw new KeyNotFoundException($"The type {typeof(T)} was not found to have a registered dependency.");
        }

        if (_singletonServices.ContainsKey(typeof(T)))
        {
            var singletonList = _singletonServices[typeof(T)];

            foreach (var singletonService in singletonList)
            {
                yield return (T)singletonService;
            }
        }

        foreach(var transientImpl in implementations.Where(i => i.Lifetime == ServiceLifetime.Transient))
        {
            yield return transientImpl.ImplementationType != null
                ? CreateService<T>(transientImpl.ImplementationType)
                : ((Func<T>)transientImpl.ImplementationFactory!)();
        }
    }

    private T CreateService<T>(Type implementationType) where T : class => DynamicObjectCreator.CreateInstance<T>(implementationType);
}
