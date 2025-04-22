using FoxholeDI.IntegrationTests.TestClasses;

namespace FoxholeDI.IntegrationTests;

public class CyclicDependencyTest
{
    [Test]
    public void DigIn_MultipleDependencySingletonsCyclicDependency_ThrowsException()
    {
        // Arrange
        var foxhole = new Foxhole();
        
        foxhole.HideForever<ILairInterface, LairClass2>();
        foxhole.HideForever<IFooInterface, FooClass1>();
        foxhole.HideForever<IDooInterface, DooClass1>();
        foxhole.HideForever<IBarInterface, BarClass1>();

        // Act & Assert
        Assert.Throws<Exception>(() => foxhole.DigIn());
    }
}