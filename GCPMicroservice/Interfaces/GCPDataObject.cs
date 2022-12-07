namespace GCPMicroservice;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
public class GCPDataObject : IDataObject
{
    private string bucketName;
    private StorageClient client = StorageClient.Create();
    public GCPDataObject(string bucketName)
    {
        this.bucketName = bucketName;
    }
    public void Create(object data)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesExist(string objectName)
    {
        Object? googleAsset = await client.GetObjectAsync(bucketName, objectName);
        
        return googleAsset is not null;
    }

    public async Task<object> Download(string path)
    {
        throw new NotImplementedException();
    }

    public void Publish(object data)
    {
        throw new NotImplementedException();
    }
}