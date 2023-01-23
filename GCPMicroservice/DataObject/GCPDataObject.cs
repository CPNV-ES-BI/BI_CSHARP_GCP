using GCPMicroservice.Exceptions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
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

        if (_client is null)
            throw new ArgumentException("GCP_TOKEN environment variable is not set");
        if (_bucket is null)
            throw new ArgumentException("GCP_BUCKET environment variable is not set");
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

    public async Task Create(string key, byte[] content, bool force = false)
    {
        if (!force && await DoesExist(key))
            throw new DataObjectAlreadyExistsException(key);

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
                    throw new DataObjectNotFoundException(e.Message, e.InnerException);
                else
                    throw;
            }
        }
    }

    public async Task<string> Publish(string key)
    {
        try
        {
            var obj = await _client.GetObjectAsync(_bucket, key);
            obj.Acl = new List<ObjectAccessControl> { new ObjectAccessControl { Entity = "allUsers", Role = "READER" } };
            _client.UpdateObject(obj);

            return obj.MediaLink;
        }
        catch (Google.GoogleApiException e)
        {
            if (e.HttpStatusCode == HttpStatusCode.NotFound)
                throw new DataObjectNotFoundException(e.Message, e.InnerException);
            else
                throw;
        }
    }

    public async Task Delete(string key, bool recursively = false)
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
                throw new DataObjectNotFoundException(e.Message, e.InnerException);
            else
                throw;
        }
    }
}