using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace GCPMicroservice;


public class GCPDataObject : IDataObject
{
    private StorageClient _client;
    private string _bucket;

    public GCPDataObject()
    {
        _client = StorageClient.Create(GoogleCredential.FromAccessToken(Environment.GetEnvironmentVariable("GCP_TOKEN")));
        _bucket = Environment.GetEnvironmentVariable("GCP_BUCKET");
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
        object result = await _client.DownloadObjectAsync(_bucket, name, destination);

        return result;
    }
    
    public void Publish(string name, object data)
    {
        Stream source = new MemoryStream();
        _client.UploadObject(_bucket, name, null, source);
    }
}