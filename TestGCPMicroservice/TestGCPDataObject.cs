namespace TestGCPMicroservice;

[TestClass]
public class TestGCPDataObject
{
    const string BUCKET = "gs://bi.csharp.gcp.cld.education/";
 
    public GCPDataObject dataObject = null!;
    
    [TestInitialize()]
    public void Startup()
    {
        dataObject = new(BUCKET);
    }

    #region DoesExist

    [TestMethod]
    public async void DoesExist_ExistsCase_True()
    {
        // Arrange
        string objectName = "test.txt";

        // Act
        bool result = await dataObject.DoesExist(objectName);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async void DoesExist_NotExists_False()
    {
        // Arrange
        string objectName = "invalid-path";

        // Act
        bool result = await dataObject.DoesExist(objectName);

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
        //Assert.IsTrue(dataObject.DoesExist("path"));
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
        //Assert.IsTrue(dataObject.DoesExist(path));
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