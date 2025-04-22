namespace FoxholeDI.UnitTests;

[TestFixture]
public class FoxholeTests
{
    [Test]
    public void Test1()
    {
        /*var justForTest = new Foxhole();
        
        justForTest.HideForever<ITestInterface, TestClass>();
        justForTest.HideForever<ITestInterface2, TestClass2>();
        
        justForTest.DigIn();*/
    }
    
    [Test]
    public void DigIn_EmptyContainer_Succeeds()
    {
        // Arrange
        var foxhole = new Foxhole();
        
        // Act & Assert
        Assert.DoesNotThrow(() => foxhole.DigIn());
    }
    
    [Test]
    public void DigIn_SingletonServiceWithNoDependencies_Succeeds()
    {
        // Arrange
        var foxhole = new Foxhole();
        foxhole.HideForever<ITestInterface2, TestClass2>();
        
        // Act & Assert
        Assert.DoesNotThrow(() => foxhole.DigIn());
    }
    
    [Test]
    public void DigIn_TransientServiceWithNoDependencies_Succeeds()
    {
        // Arrange
        var foxhole = new Foxhole();
        foxhole.RoamFree<ITestInterface2, TestClass2>();
        
        // Act & Assert
        Assert.DoesNotThrow(() => foxhole.DigIn());
    }

    [Test]
    public void DigIn_SingletonServiceWithDependenciesDependencyNotPresent_ThrowsException()
    {
        // Arrange
        var foxhole = new Foxhole();
        foxhole.HideForever<ITestInterface, TestClass>();
        
        // Act & Assert
        Assert.Throws<Exception>(() => foxhole.DigIn());
    }
}

public interface ITestInterface
{
    public string TestMethod();
}

public interface ITestInterface2
{
    public string TestMethod();
}

public class TestClass : ITestInterface
{
    private readonly ITestInterface2 _testInterface2;

    public TestClass(ITestInterface2 testInterface2)
    {
        _testInterface2 = testInterface2 ?? throw new ArgumentNullException(nameof(testInterface2));
    }
    
    public string TestMethod()
    {
        return "Hello from TestClass!";
    }
}

public class TestClass2 : ITestInterface2
{
    public TestClass2()
    {
        
    }
    
    public string TestMethod()
    {
        return "Hello from TestClass2!";
    }
}