namespace GCPMicroservice;

public interface IDataObject
{
    public Task<bool> DoesExist(string key);

    public Task Create(string key, byte[] content);

    public Task<byte[]> Download(string key);

    public void Publish(string name, object data);

    public Task Delete(string key);
}