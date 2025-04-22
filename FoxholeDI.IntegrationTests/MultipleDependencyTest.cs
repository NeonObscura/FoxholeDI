using FoxholeDI.IntegrationTests.TestClasses;

namespace FoxholeDI.IntegrationTests;

public class MultipleDependencyTest
{
    [Test]
    public void DigIn_MultipleDependencySingletons_DoesNotThrow()
    {
        // Arrange
        var foxhole = new Foxhole();
        
        foxhole.HideForever<ILairInterface, LairClass1>();
        foxhole.HideForever<IFooInterface, FooClass1>();
        foxhole.HideForever<IDooInterface, DooClass1>();
        foxhole.HideForever<IBarInterface, BarClass1>();

        // Act & Assert
        Assert.DoesNotThrow(() => foxhole.DigIn());
    }
    
    [Test]
    public void DigIn_MultipleDependencyTransients_DoesNotThrow()
    {
        // Arrange
        var foxhole = new Foxhole();
        
        foxhole.RoamFree<ILairInterface, LairClass1>();
        foxhole.RoamFree<IFooInterface, FooClass1>();
        foxhole.RoamFree<IDooInterface, DooClass1>();
        foxhole.RoamFree<IBarInterface, BarClass1>();

        // Act & Assert
        Assert.DoesNotThrow(() => foxhole.DigIn());
    }
}