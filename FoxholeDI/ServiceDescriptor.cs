namespace FoxholeDI;

internal class ServiceDescriptor
{
    public Type ServiceType { get; set; }
    public Type ImplementationType { get; set; }
    public ServiceLifetimeType Lifetime { get; set; }
    
    public List<ServiceDescriptor> Dependencies { get; set; } = new List<ServiceDescriptor>();
    public bool IsResolved { get; set; }
    
    public object ImplementationInstance { get; set; }
}