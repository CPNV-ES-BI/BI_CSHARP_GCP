using System.Text;
using GCPMicroservice.Services;
using GCPMicroservice.Exceptions;

namespace TestGCPMicroservice;

[TestClass]
public class TestGCPDataObject
{
    private const string Path     = "tests/";
    private const string Key      = "object.txt";
    private const string FullKey = Path + Key;

    private readonly byte[] Content = Encoding.UTF8.GetBytes("content of the file");

    private GCPDataObject _dataObject = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string dotenv = System.IO.Path.Combine(projectDirectory, ".env");
        DotEnv.Load(dotenv);
    }
    
    [TestInitialize]
    public async Task Startup()
    {
        _dataObject = new ();
        await _dataObject.Delete(Path, true);
    }

    #region DoesExist

    [TestMethod]
    public async Task DoesExist_ExistsCase_True()
    {
        // Arrange
        await _dataObject.Create(FullKey, Content);

        // Act
        bool responseExist = await _dataObject.DoesExist(FullKey);

        // Assert
        Assert.IsTrue(responseExist);
    }

    [TestMethod]
    public async Task DoesExist_NotExists_False()
    {
        // Arrange
        string key = Path + "invalid-key";

        // Act
        bool responseExist = await _dataObject.DoesExist(key);

        // Assert
        Assert.IsFalse(responseExist);
    }

    #endregion

    #region Create Object

    [TestMethod]
    public async Task CreateObject_NominalCase_ObjectExists()
    {
        // Arrange
        
        // Act
        await _dataObject.Create(FullKey, Content);

        bool responseExist = await _dataObject.DoesExist(FullKey);
        byte[] responseContent = await _dataObject.Download(FullKey);

        // Assert
        Assert.IsTrue(responseExist);
        Assert.IsTrue(Content.SequenceEqual(responseContent));
    }

    [TestMethod]
    public async Task CreateObject_AlreadyExists_ThrowException()
    {
        // Arrange
        await _dataObject.Create(FullKey, Content);
        bool exist = await _dataObject.DoesExist(FullKey);

        Assert.IsTrue(exist);

        // Act
        await Assert.ThrowsExceptionAsync<DataObjectAlreadyExistsException>(async () => 
            await _dataObject.Create(FullKey, Content));

        // Assert
        // Exception is thrown
    }

    [TestMethod]
    public async Task CreateObject_ForceAlreadyExists_ObjectExists()
    {
        // Arrange
        byte[] content = Encoding.UTF8.GetBytes("updated content");

        await _dataObject.Create(FullKey, Content);
        bool exist = await _dataObject.DoesExist(FullKey);

        Assert.IsTrue(exist);

        // Act
        await _dataObject.Create(FullKey, content, true);
        bool responseExist = await _dataObject.DoesExist(FullKey);
        byte[] responseContent = await _dataObject.Download(FullKey); 

        // Assert
        Assert.IsTrue(responseExist);
        Assert.IsTrue(content.SequenceEqual(responseContent));
    }

    [TestMethod]
    public async Task CreateObject_PathNotExists_ObjectExists()
    {
        // Arrange
        string key = Path + "not_existing_path/" + Key;

        // Act
        await _dataObject.Create(key, Content);

        bool responseExist = await _dataObject.DoesExist(key);
        byte[] responseContent = await _dataObject.Download(key);

        // Assert
        Assert.IsTrue(responseExist);
        Assert.IsTrue(Content.SequenceEqual(responseContent));
    }

    #endregion

    #region Download Object

    [TestMethod]
    public async Task DownloadObject_NominalCase_Downloaded()
    {
        // Arrange
        await _dataObject.Create(FullKey, Content);
        bool exist = await _dataObject.DoesExist(FullKey);

        Assert.IsTrue(exist);

        // Act
        byte[] responseContent = await _dataObject.Download(FullKey);

        // Assert
        Assert.IsTrue(Content.SequenceEqual(responseContent));
    }

    [TestMethod]
    public async Task DownloadObject_NotExists_ThrowException()
    {
        // Arrange
        string key = Path + "invalid-key";
        bool exist = await _dataObject.DoesExist(key);

        Assert.IsFalse(exist);

        // Act
        await Assert.ThrowsExceptionAsync<DataObjectNotFoundException>(async () =>
            await _dataObject.Download(key));

        // Assert
        // Throw an exception
    }

    #endregion

    #region Publish Object

    [TestMethod]
    public async Task PublishObject_NominalCase_ObjectPublished()
    {
        // Arrange
        await _dataObject.Create(FullKey, Content);
        bool exist = await _dataObject.DoesExist(FullKey);

        Assert.IsTrue(exist);

        // Act
        string url = await _dataObject.Publish(FullKey);

        // Assert
        using HttpClient client = new();
        var responseUrl = await client.GetAsync(url);
        var responseContent = await responseUrl.Content.ReadAsByteArrayAsync();

        // Then
        Assert.IsTrue(Content.SequenceEqual(responseContent));
    }

    [TestMethod]
    public async Task PublishObject_ObjectNotFound_ThrowException()
    {
        // Arrange

        // Act

        await Assert.ThrowsExceptionAsync<DataObjectNotFoundException>(async () => await _dataObject.Publish(FullKey));

        // Asserts
        // Throw an exception   
    }

    #endregion

    #region Delete Object

    [TestMethod]
    public async Task DeleteObject_ObjectExists_ObjectDeleted()
    {
        // Arrange
        await _dataObject.Create(FullKey, Content);
        bool exist = await _dataObject.DoesExist(FullKey);

        Assert.IsTrue(exist);

        // Act
        await _dataObject.Delete(FullKey);
        bool responseExist = await _dataObject.DoesExist(FullKey);

        // Assert
        Assert.IsFalse(responseExist);
    }

    [TestMethod]
    public async Task DeleteObject_ObjectContainingSubObjectsExists_ObjectDeletedRecursively()
    {
        // Arrange
        string parent = "object-parent/";
        string[] childs = { "object1.txt", "object2.txt" };
        
        foreach (string child in childs)
        {
            await _dataObject.Create(Path + parent + child, Content);
            bool exist = await _dataObject.DoesExist(Path + parent + child);
        
            Assert.IsTrue(exist);
        }

        // Act
        await _dataObject.Delete(Path + parent, true);

        // Assert
        foreach (string child in childs)
        {
            bool responseExist = await _dataObject.DoesExist(Path + parent + child);
            Assert.IsFalse(responseExist);
        }
    }

    [TestMethod]
    public async Task DeleteObject_ObjectDoesntExist_ThrowException()
    {
        // Arrange
        string key = Path + "invalid-key";
        bool exist = await _dataObject.DoesExist(key);

        Assert.IsFalse(exist);

        // Act
        await Assert.ThrowsExceptionAsync<DataObjectNotFoundException>(async () =>
            await _dataObject.Delete(key));

        // Assert
        // Exception is thrown
    }

    #endregion
}