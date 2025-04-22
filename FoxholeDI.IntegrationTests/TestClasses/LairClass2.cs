namespace FoxholeDI.IntegrationTests.TestClasses;

public class LairClass2 : ILairInterface
{
    private readonly IFooInterface _foo;

    public LairClass2(IFooInterface foo)
    {
        _foo = foo ?? throw new ArgumentNullException(nameof(foo));
    }
    
    public void DoLairStuff()
    {
    }
}