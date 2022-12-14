namespace GCPMicroservice;
using Google.Cloud.Storage.V1;
using System.IO;

public class GCPDataObject : IDataObject
{
    private StorageClient _client = StorageClient.Create();
    private string _bucket;
    
    public GCPDataObject(string bucket)
    {
        _bucket = bucket;
    }
    
    public void Create(object data)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesExist(string name)
    {
        return await _client.GetObjectAsync(_bucket, name) is not null;
    }

    public async Task<object> Download(string name)
    {
        Stream destination = new MemoryStream();
        var obj = await _client.DownloadObjectAsync(_bucket, name, destination);
        return obj;
    }
    
    public void Publish(string name, object data)
    {
        Stream source = new MemoryStream();
        _client.UploadObject(_bucket, name, null, source);
    }
}