namespace GCPMicroservice;

public interface IDataObject
{
    public Task<bool> DoesExist(string key);

    public Task Create(string key, byte[] content, bool force);

    public Task<byte[]> Download(string key);

    public Task<string> Publish(string key);

    public Task Delete(string key);
}