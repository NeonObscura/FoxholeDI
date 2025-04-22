namespace FoxholeDI.Dependency;

internal class DependencyTreeMember
{
    public ServiceDescriptor Descriptor { get; set; }
    public IEnumerable<DependencyTreeMember> DependsOn { get; set; }
}