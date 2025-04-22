using System.Reflection;

namespace FoxholeDI;

public class Foxhole
{
    private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptorsMap;
    private readonly Dictionary<Type, object> _singletonInstancesMap = new();
    
    public T SniffOut<T>() => (T)SniffOut(typeof(T));

    public Foxhole()
    {
        _serviceDescriptorsMap = new Dictionary<Type, ServiceDescriptor>();
    }
    
    private object SniffOut(Type type)
    {
        if (_singletonInstancesMap.TryGetValue(type, out var instance))
            return instance;
        
        if (!_serviceDescriptorsMap.TryGetValue(type, out var serviceDescriptor))
            throw new Exception($"Service {type} is not registered. FoxholeDI requires all dependencies to be registered.");

        return BuildObject(serviceDescriptor);
    }

    private object BuildObject(ServiceDescriptor descriptor)
    {
        if (descriptor.Lifetime == ServiceLifetimeType.Singleton)
            return BuildSingletonObject(descriptor);
        
        var dependencies = new object[descriptor.Dependencies.Count];
        for (int index = 0; index < descriptor.Dependencies.Count; index++)
        {
            var dependency = descriptor.Dependencies[index];
            dependencies[index] = BuildObject(dependency);
        }
        
        return descriptor.ImplementationType.GetConstructors()[0].Invoke(dependencies);
    }

    private object BuildSingletonObject(ServiceDescriptor descriptor)
    {
        var dependencies = new object[descriptor.Dependencies.Count];
        for (int index = 0; index < descriptor.Dependencies.Count; index++)
        {
            var dependency = descriptor.Dependencies[index];
            dependencies[index] = BuildSingletonObject(dependency);
        }
        
        var builtObject = descriptor.ImplementationType.GetConstructors()[0].Invoke(dependencies);
        _singletonInstancesMap.Add(descriptor.ServiceType, builtObject);
        return builtObject;
    }
    
    public void DigIn()
    {
        foreach (var serviceDescriptor in _serviceDescriptorsMap.Values)
        {
            if(serviceDescriptor.IsResolved)
                continue;

            BuildDependenciesTree(serviceDescriptor);
        }
        
        CheckCircularDependencies();
        CheckLifespanHierarchies();
    }
    
    private void BuildDependenciesTree(ServiceDescriptor serviceDescriptor)
    {
        var constructors = serviceDescriptor.ImplementationType.GetConstructors();
            
        if (constructors.Length == 0)
            throw new Exception($"Service {serviceDescriptor.ServiceType} has no public constructor. FoxholeDI requires at least one constructor.");
            
        if(constructors.Length != 1)
            throw new Exception($"Service {serviceDescriptor.ServiceType} has more than one public constructors. FoxholeDI only supports single constructor DI.");
        
        var constructor = constructors[0];
        var parameters = constructor.GetParameters();

        foreach (var parameter in parameters)
        {
            if(!_serviceDescriptorsMap.TryGetValue(parameter.ParameterType, out var parameterDescriptor))
                throw new Exception($"Service {serviceDescriptor.ServiceType} has a constructor parameter of type {parameter.ParameterType}, but it is not registered in the container. FoxholeDI requires all dependencies to be registered.");
                
            serviceDescriptor.Dependencies.Add(parameterDescriptor);
        }
    }

    void CheckCircularDependencies()
    {
        foreach (var serviceDescriptor in _serviceDescriptorsMap.Values)
        {
            if(CheckCircularDependency(serviceDescriptor))
                throw new Exception($"Circular dependency detected for service {serviceDescriptor.ServiceType}. FoxholeDI does not support circular dependencies.");
        }
    }

    private bool CheckCircularDependency(ServiceDescriptor serviceDescriptor)
    {
        var visited = new HashSet<ServiceDescriptor>();
        var stack = new Stack<ServiceDescriptor>();
        stack.Push(serviceDescriptor);

        while (stack.Any())
        {
            var current = stack.Pop();
            if (!visited.Add(current))
                return true;

            foreach (var dependency in current.Dependencies)
            {
                stack.Push(dependency);
            }
        }

        return false;
    }

    void CheckLifespanHierarchies()
    {
        foreach (var serviceDescriptor in _serviceDescriptorsMap.Values)
        {
            CheckLifespanHierarchy(serviceDescriptor);
        }
    }

    private void CheckLifespanHierarchy(ServiceDescriptor serviceDescriptor)
    {
        var current = serviceDescriptor.Lifetime;
        var stack = new Stack<ServiceDescriptor>();
        stack.Push(serviceDescriptor);
        
        while (stack.Any())
        {
            var currentDescriptor = stack.Pop();
            if(currentDescriptor.Lifetime > current)
                throw new Exception($"Service {currentDescriptor.ServiceType} has a longer lifespan than its dependencies. FoxholeDI does not support this.");
            
            current = currentDescriptor.Lifetime;
            
            foreach (var dependency in currentDescriptor.Dependencies)
            {
                stack.Push(dependency);
            }
        }
    }

    public void HideForever<TService, TImplementation>() 
        where TImplementation : class, TService
    {
        RegisterService(typeof(TService), typeof(TImplementation), ServiceLifetimeType.Singleton);
    }

    public void RoamFree<TService, TImplementation>() 
        where TImplementation : class, TService
    {
        RegisterService(typeof(TService), typeof(TImplementation), ServiceLifetimeType.Transient);
    }

    private void RegisterService(Type serviceType, Type implementationType, ServiceLifetimeType lifetime)
    {
        if(_serviceDescriptorsMap.ContainsKey(serviceType))
            throw new Exception($"Service {serviceType} is already registered. FoxholeDI does not support multiple registrations of the same service.");
        
        var serviceDescriptor = new ServiceDescriptor
        {
            ServiceType = serviceType,
            ImplementationType = implementationType,
            Lifetime = lifetime
        };

        _serviceDescriptorsMap.Add(serviceType, serviceDescriptor);
    }
}