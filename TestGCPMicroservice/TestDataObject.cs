using MyMicroservice;

namespace TestGCPMicroservice;

[TestClass]
public class TestDataObject
{
    public DataObject dataObject = null!;
    
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
        string path = "valid-path";

        // Act
        bool result = dataObject.DoesExist(path);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DoesExist_NotExists_False()
    {
        // Arrange
        string path = "invalid-path";
        
        // Act
        bool result = dataObject.DoesExist(path);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion

    #region Create Object

    [TestMethod]
    public void CreateObject_NominalCase_ObjectExists()
    {
        // Arrange
        object data = new();

        // Act
        dataObject.Create(data);

        // Assert
        Assert.IsTrue(dataObject.DoesExist("path"));
    }

    [TestMethod]
    public void CreateObject_AlreadyExists_ThrowException()
    {
        // Arrange
        object data = new();

        // Act
        Assert.ThrowsException<Exception>(() => dataObject.Create(data));

        // Assert
        // Throw an exception
    }

    [TestMethod]
    public void CreateObject_PathNotExists_ObjectExists()
    {
        // Arrange

        // Act

        // Assert
    }

    #endregion

    #region Download Object

    [TestMethod]
    public void DownloadObject_NominalCase_Downloaded()
    {
        // Arrange
        string path = "valid-path";

        // Act
        object data = dataObject.Download(path);

        // Assert
        Assert.IsNotNull(data);
    }

    [TestMethod]
    public void DownloadObject_NotExists_ThrowException()
    {
        // Arrange
        string path = "invalid-path";

        // Act
        Assert.ThrowsException<Exception>(() => dataObject.Download(path));

        // Assert
        // Throw an exception
    }

    #endregion

    #region Publish Object

    [TestMethod]
    public void PublishObject_NominalCase_ObjectPublished()
    {
        // Arrange
        string path = "valid-path";

        // Act
        dataObject.Publish(path);

        // Assert
        Assert.IsTrue(dataObject.DoesExist(path));
    }

    [TestMethod]
    public void PublishObject_ObjectNotFound_ThrowException()
    {
        // Arrange
        string path = "invalid-path";

        // Act
        Assert.ThrowsException<Exception>(() => dataObject.Publish(path));

        // Assert
        // Throw an exception
    }

    #endregion
}
