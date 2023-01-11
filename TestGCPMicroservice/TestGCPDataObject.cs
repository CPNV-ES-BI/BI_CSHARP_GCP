using Microsoft.Extensions.Configuration;

namespace TestGCPMicroservice;

[TestClass]
public class TestGCPDataObject
{
    public GCPDataObject dataObject = null!;
    
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string dotenv = Path.Combine(projectDirectory, ".env");
        DotEnv.Load(dotenv);
    }
    
    [TestInitialize]
    public void Startup()
    {
        dataObject = new ();
    }

    #region DoesExist

    [TestMethod]
    public async Task DoesExist_ExistsCase_True()
    {
        // Arrange
        string objectName = "test.txt";

        // Act
        bool result = await dataObject.DoesExist(objectName);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DoesExist_NotExists_False()
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
        // dataObject.Create(data);

        // Assert
        //Assert.IsTrue(dataObject.DoesExist("path"));
    }

    [TestMethod]
    public void CreateObject_AlreadyExists_ThrowException()
    {
        // Arrange
        object data = new();

        // Act
        // Assert.ThrowsException<Exception>(() => dataObject.Create(data));

        // Assert
        // Throw an exception
    }

    [TestMethod]
    public void CreateObject_PathNotExists_ObjectExists()
    {
        // Arrange
        object data = new object();
        string path = "valid-path";

        // Act
        // dataObject.Create(data, path);

        // Assert
        // Assert.IsTrue(dataObject.DoesExist(path));
    }

    #endregion

    #region Download Object

    [TestMethod]
    public async Task DownloadObject_NominalCase_Downloaded()
    {
        // Arrange
        string name = "test.txt";

        // Act
        object data = await dataObject.Download(name);

        // Assert
        Assert.IsNotNull(data);
    }

    [TestMethod]
    public void DownloadObject_NotExists_ThrowException()
    {
        // Arrange
        string name = "invalid-name";

        // Act
        Assert.ThrowsException<Exception>(() => dataObject.Download(name));

        // Assert
        // Throw an exception
    }

    #endregion

    #region Publish Object

    [TestMethod]
    public void PublishObject_NominalCase_ObjectPublished()
    {
        // Arrange
        string name = "valid-name";
        object data = new();

        // Act
        dataObject.Publish(name, data);

        // Assert
        //Assert.IsTrue(dataObject.DoesExist(path));
    }

    [TestMethod]
    public void PublishObject_ObjectNotFound_ThrowException()
    {
        // Arrange
        string name = "valid-name";
        object data = new();

        // Act
        Assert.ThrowsException<Exception>(() => dataObject.Publish(name, data));

        // Assert
        // Throw an exception   
    }

    #endregion
}