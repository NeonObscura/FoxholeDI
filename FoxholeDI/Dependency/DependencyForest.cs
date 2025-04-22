namespace FoxholeDI.Dependency;

internal class DependencyForest
{
    private Dictionary<Type, DependencyTreeMember> _dependencyTreeLookup = new Dictionary<Type, DependencyTreeMember>();
    
    public void AddDependencyTree(DependencyTreeMember tree)
    {
        if (!_dependencyTreeLookup.TryAdd(tree.Descriptor.ServiceType, tree))
            return;

        foreach (var treeMember in tree.DependsOn)
        {
            AddDependencyTree(treeMember);
        }
    }
    
    public bool TryGetDependencyTree(Type serviceType, out DependencyTreeMember tree)
    {
        return _dependencyTreeLookup.TryGetValue(serviceType, out tree);
    }
}