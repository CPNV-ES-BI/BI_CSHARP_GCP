using GCPMicroservice.Exceptions;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml.Linq;

namespace TestGCPMicroservice;

[TestClass]
public class TestGCPDataObject
{
    private const string PATH = "tests/";
    private const string KEY = PATH + "object.txt";
    private readonly byte[] CONTENT = Encoding.UTF8.GetBytes("content of the file");

    private GCPDataObject _dataObject = null!;

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
        _dataObject = new ();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        if (await _dataObject.DoesExist(KEY))
            await _dataObject.Delete(KEY);
    }

    #region DoesExist

    [TestMethod]
    public async Task DoesExist_ExistsCase_True()
    {
        // Arrange
        await _dataObject.Create(KEY, CONTENT);

        // Act
        bool result = await _dataObject.DoesExist(KEY);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DoesExist_NotExists_False()
    {
        // Arrange
        string key = "invalid-key";

        // Act
        bool result = await _dataObject.DoesExist(key);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion

    #region Create Object

    [TestMethod]
    public async Task CreateObject_NominalCase_ObjectExists()
    {
        // Arrange
        
        // Act
        await _dataObject.Create(KEY, CONTENT);
        bool exist = await _dataObject.DoesExist(KEY);

        // Assert
        Assert.IsTrue(exist);
    }

    [TestMethod]
    public async Task CreateObject_AlreadyExists_ThrowException()
    {
        // Arrange
        await _dataObject.Create(KEY, CONTENT);
        bool exist = await _dataObject.DoesExist(KEY);

        Assert.IsTrue(exist);

        // Act
        await Assert.ThrowsExceptionAsync<DataObjectAlreadyExistsException>(async () => 
            await _dataObject.Create(KEY, CONTENT));

        // Assert
        // Exception is thrown
    }

    [TestMethod]
    public async Task CreateObject_PathNotExists_ObjectExists()
    {
        // Arrange
        string key = PATH + "not_existing_path/object.txt";

        // Act
        await _dataObject.Create(key, CONTENT);
        bool exist = await _dataObject.DoesExist(key);

        // Assert
        Assert.IsTrue(exist);

        // Cleanup
        await _dataObject.Delete(key);
    }

    #endregion

    #region Download Object

    [TestMethod]
    public async Task DownloadObject_NominalCase_Downloaded()
    {
        // Arrange
        string name = "test.txt";

        // Act
        object data = await _dataObject.Download(name);

        // Assert
        Assert.IsNotNull(data);
    }

    [TestMethod]
    public void DownloadObject_NotExists_ThrowException()
    {
        // Arrange
        string name = "invalid-name";

        // Act
        Assert.ThrowsException<Exception>(() => _dataObject.Download(name));

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
        _dataObject.Publish(name, data);

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
        Assert.ThrowsException<Exception>(() => _dataObject.Publish(name, data));

        // Assert
        // Throw an exception   
    }

    #endregion

    #region Delete Object

    [TestMethod]
    public async Task DeleteObject_ObjectExists_ObjectDeleted()
    {
        // Arrange
        bool exist;

        await _dataObject.Create(KEY, CONTENT);
        exist = await _dataObject.DoesExist(KEY);
        Assert.IsTrue(exist);

        // Act
        await _dataObject.Delete(KEY);

        // Assert
        exist = await _dataObject.DoesExist(KEY);
        Assert.IsFalse(exist);
    }

    [TestMethod]
    public async Task DeleteObject_ObjectContainingSubObjectsExists_ObjectDeletedRecursively()
    {
        // Arrange

        // Act

        // Assert
    }

    [TestMethod]
    public async Task DeleteObject_ObjectDoesntExist_ThrowException()
    {
        // Arrange

        // Act

        // Assert
        // Exception is thrown
    }

    #endregion
}