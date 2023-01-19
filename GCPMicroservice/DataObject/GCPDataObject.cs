using GCPMicroservice.Exceptions;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Net;

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

    public async Task<bool> DoesExist(string key)
    {
        try
        {
            await _client.GetObjectAsync(_bucket, key);
            return true;
        }
        catch (Google.GoogleApiException e)
        {
            if (e.HttpStatusCode == HttpStatusCode.NotFound)
                return false;
            else
                throw;
        }
    }

    public async Task Create(string key, byte[] content)
    {
        if (await DoesExist(key))
            throw new DataObjectAlreadyExistsException();

        using (var stream = new MemoryStream(content))
        {
            await _client.UploadObjectAsync(_bucket, key, "text/plain", stream);
        }
    }

    public async Task<byte[]> Download(string key)
    {
        using (var destination = new MemoryStream())
        {
            try
            {
                await _client.DownloadObjectAsync(_bucket, key, destination);
                return destination.ToArray();
            }
            catch (Google.GoogleApiException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.NotFound)
                    throw new DataObjectNotFoundException();
                else
                    throw;
            }
        }
    }

    public void Publish(string name, object data)
    {
        Stream source = new MemoryStream();
        _client.UploadObject(_bucket, name, null, source);
    }

    public async Task Delete(string key)
    {
        await TryToDelete(key);
    }

    public async Task Delete(string key, bool recursively)
    {
        if (recursively)
        {
            var objects = _client.ListObjectsAsync(_bucket, key);

            await foreach (var o in objects)
            {
                await TryToDelete(o.Name);
            }
        }
        else
        {
            await TryToDelete(key);
        }
    }

    private async Task TryToDelete(string key)
    {
        try
        {
            await _client.DeleteObjectAsync(_bucket, key);
        }
        catch (Google.GoogleApiException e)
        {
            if (e.HttpStatusCode == HttpStatusCode.NotFound)
                throw new DataObjectNotFoundException();
            else
                throw;
        }
    }
}