using GCPMicroservice.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Text;

namespace TestGCPMicroservice;

[TestClass]
public class TestGCPDataObject
{
    private const string PATH     = "tests/";
    private const string KEY      = "object.txt";
    private const string FULL_KEY = PATH + KEY;

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
    public async Task Startup()
    {
        _dataObject = new ();
        await _dataObject.Delete(PATH, true);
    }

    #region DoesExist

    [TestMethod]
    public async Task DoesExist_ExistsCase_True()
    {
        // Arrange
        await _dataObject.Create(FULL_KEY, CONTENT);

        // Act
        bool result = await _dataObject.DoesExist(FULL_KEY);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DoesExist_NotExists_False()
    {
        // Arrange
        string key = PATH + "invalid-key";

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
        await _dataObject.Create(FULL_KEY, CONTENT);

        bool responseExist = await _dataObject.DoesExist(FULL_KEY);
        byte[] responseContent = await _dataObject.Download(FULL_KEY);

        // Assert
        Assert.IsTrue(responseExist);
        Assert.IsTrue(CONTENT.SequenceEqual(responseContent));
    }

    [TestMethod]
    public async Task CreateObject_AlreadyExists_ThrowException()
    {
        // Arrange
        await _dataObject.Create(FULL_KEY, CONTENT);
        bool exist = await _dataObject.DoesExist(FULL_KEY);

        Assert.IsTrue(exist);

        // Act
        await Assert.ThrowsExceptionAsync<DataObjectAlreadyExistsException>(async () => 
            await _dataObject.Create(FULL_KEY, CONTENT));

        // Assert
        // Exception is thrown
    }

    [TestMethod]
    public async Task ForceCreateObject_AlreadyExists_ObjectExists()
    {
        // Arrange
        byte[] content = Encoding.UTF8.GetBytes("updated content");

        await _dataObject.Create(FULL_KEY, CONTENT);
        bool exist = await _dataObject.DoesExist(FULL_KEY);

        Assert.IsTrue(exist);

        // Act
        await _dataObject.Create(FULL_KEY, content, true);
        bool responseExist = await _dataObject.DoesExist(FULL_KEY);
        byte[] responseContent = await _dataObject.Download(FULL_KEY); 

        // Assert
        Assert.IsTrue(responseExist);
        Assert.IsTrue(content.SequenceEqual(responseContent));
    }

    [TestMethod]
    public async Task CreateObject_PathNotExists_ObjectExists()
    {
        // Arrange
        string key = PATH + "not_existing_path/" + KEY;

        // Act
        await _dataObject.Create(key, CONTENT);

        bool responseExist = await _dataObject.DoesExist(key);
        byte[] responseContent = await _dataObject.Download(key);

        // Assert
        Assert.IsTrue(responseExist);
        Assert.IsTrue(CONTENT.SequenceEqual(responseContent));
    }

    #endregion

    #region Download Object

    [TestMethod]
    public async Task DownloadObject_NominalCase_Downloaded()
    {
        // Arrange
        await _dataObject.Create(FULL_KEY, CONTENT);
        bool exist = await _dataObject.DoesExist(FULL_KEY);

        Assert.IsTrue(exist);

        // Act
        byte[] obj = await _dataObject.Download(FULL_KEY);

        // Assert
        Assert.IsTrue(CONTENT.SequenceEqual(obj));
    }

    [TestMethod]
    public async Task DownloadObject_NotExists_ThrowException()
    {
        // Arrange
        string key = PATH + "invalid-key";
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
        await _dataObject.Create(FULL_KEY, CONTENT);
        bool exist = await _dataObject.DoesExist(FULL_KEY);

        Assert.IsTrue(exist);

        // Act
        string url = await _dataObject.Publish(FULL_KEY);

        // Assert
        using (HttpClient client = new())
        {
            var response = await client.GetAsync(url);
            var obj = await response.Content.ReadAsByteArrayAsync();

            // Then
            Assert.IsTrue(CONTENT.SequenceEqual(obj));
        }
    }

    [TestMethod]
    public async Task PublishObject_ObjectNotFound_ThrowException()
    {
        // Arrange

        // Act

        await Assert.ThrowsExceptionAsync<DataObjectNotFoundException>(async () => await _dataObject.Publish(FULL_KEY));

        // Asserts
        // Throw an exception   
    }

    #endregion

    #region Delete Object

    [TestMethod]
    public async Task DeleteObject_ObjectExists_ObjectDeleted()
    {
        // Arrange
        bool exist;

        await _dataObject.Create(FULL_KEY, CONTENT);
        exist = await _dataObject.DoesExist(FULL_KEY);
        Assert.IsTrue(exist);

        // Act
        await _dataObject.Delete(FULL_KEY);

        // Assert
        exist = await _dataObject.DoesExist(FULL_KEY);
        Assert.IsFalse(exist);
    }

    [TestMethod]
    public async Task DeleteObject_ObjectContainingSubObjectsExists_ObjectDeletedRecursively()
    {
        // Arrange
        string parent = "object-parent/";
        string[] childs = { "object1.txt", "object2.txt" };
        
        bool exist;

        foreach (string child in childs)
        {
            await _dataObject.Create(PATH + parent + child, CONTENT);
            exist = await _dataObject.DoesExist(PATH + parent + child);
        
            Assert.IsTrue(exist);
        }

        // Act
        await _dataObject.Delete(PATH + parent, true);

        // Assert
        foreach (string child in childs)
        {
            exist = await _dataObject.DoesExist(PATH + parent + child);
            Assert.IsFalse(exist);
        }
    }

    [TestMethod]
    public async Task DeleteObject_ObjectDoesntExist_ThrowException()
    {
        // Arrange
        string key = PATH + "invalid-key";
        bool result = await _dataObject.DoesExist(key);

        Assert.IsFalse(result);

        // Act
        await Assert.ThrowsExceptionAsync<DataObjectNotFoundException>(async () =>
            await _dataObject.Delete(key));

        // Assert
        // Exception is thrown
    }

    #endregion
}