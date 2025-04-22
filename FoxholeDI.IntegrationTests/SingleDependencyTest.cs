using FoxholeDI.IntegrationTests.TestClasses;

namespace FoxholeDI.IntegrationTests;

[TestFixture]
public class SingleDependencyTest
{
    [Test]
    public void DigIn_SingleSingletonDependencyWithoutDependencies_DoesNotThrow()
    {
        // Arrange
        var foxhole = new Foxhole();
        foxhole.HideForever<ILairInterface, LairClass1>();

        // Act & Assert
        Assert.DoesNotThrow(() => foxhole.DigIn());
    }
    
    [Test]
    public void DigIn_SingleTransientDependencyWithoutDependencies_DoesNotThrow()
    {
        // Arrange
        var foxhole = new Foxhole();
        foxhole.RoamFree<ILairInterface, LairClass1>();

        // Act & Assert
        Assert.DoesNotThrow(() => foxhole.DigIn());
    }
    
    [Test]
    public void DigIn_SingleSingletonDependencyMissingDependency_ThrowsException()
    {
        // Arrange
        var foxhole = new Foxhole();
        foxhole.HideForever<ILairInterface, LairClass2>();

        // Act & Assert
        Assert.Throws<Exception>(() => foxhole.DigIn());
    }
    
    [Test]
    public void DigIn_SingleTransientDependencyMissingDependency_ThrowsException()
    {
        // Arrange
        var foxhole = new Foxhole();
        foxhole.RoamFree<ILairInterface, LairClass2>();

        // Act & Assert
        Assert.Throws<Exception>(() => foxhole.DigIn());
    }
}