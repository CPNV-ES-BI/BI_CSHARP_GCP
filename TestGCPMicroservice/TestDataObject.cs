using MyMicroservice;

namespace TestGCPMicroservice;

[TestClass]
public class TestDataObject
{
    public DataObject dataObject = null!;
    
    [ClassInitialize]
    public void Setup(TestContext context)
    {
        dataObject = new();
    }

    [TestMethod]
    public void DoesExist_ExistsCase_True()
    {
        // Arrange
        var path = "valid-path";

        // Act
        var result = dataObject.DoesExist(path);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DoesExist_NotExists_False()
    {
        // Arrange
        var path = "invalid-path";

        // Act
        var result = dataObject.DoesExist(path);

        // Assert
        Assert.IsFalse(result);
    }
}