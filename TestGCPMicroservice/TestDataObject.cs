using MyMicroservice;

namespace TestGCPMicroservice;

[TestClass]
public class TestDataObject
{
    public DataObject dataObject = null!;
    
    // Before each

    [TestInitialize()]
    public void Startup()
    {
        dataObject = new();
    }

    #region DoesExist

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

    #endregion

    #region Create Object

    [TestMethod]
    public void CreateObject_NominalCase_ObjectExists()
    {
        // Arrange
        var data = new object();

        // Act
        dataObject.Create(data);

        // Assert
        Assert.IsTrue(dataObject.DoesExist("path"));
    }

    [TestMethod]
    public void CreateObject_AlreadyExists_ThrowException()
    {
        // Arrange
        var data = new object();

        // Act
        dataObject.Create(data);

        // Assert
        Assert.ThrowsException<Exception>(() => dataObject.Create(data));
    }

    [TestMethod]
    public void CreateObject_PathNotExists_ObjectExists()
    {
        // Arrange

        // Act

        // Assert
    }

    #endregion

}
