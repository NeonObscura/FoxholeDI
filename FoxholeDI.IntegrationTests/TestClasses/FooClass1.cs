namespace FoxholeDI.IntegrationTests.TestClasses;

public class FooClass1 : IFooInterface
{
    private readonly IDooInterface _doo;
    private readonly IBarInterface _bar;

    public FooClass1(IDooInterface doo, IBarInterface bar)
    {
        _doo = doo ?? throw new ArgumentNullException(nameof(doo));
        _bar = bar ?? throw new ArgumentNullException(nameof(bar));
    }
    
    public void DoSomething()
    {
        
    }
}