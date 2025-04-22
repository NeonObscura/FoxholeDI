namespace FoxholeDI.IntegrationTests.TestClasses;

public class BarClass1 : IBarInterface
{
    private readonly ILairInterface _lair;

    public BarClass1(ILairInterface lair)
    {
        _lair = lair ?? throw new ArgumentNullException(nameof(lair));
    }
    
    public void DoSomethingElse()
    {
    }
}