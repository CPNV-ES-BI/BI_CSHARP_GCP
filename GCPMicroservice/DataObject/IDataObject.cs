namespace GCPMicroservice;

public interface IDataObject
{
    public Task<bool> DoesExist(string key);

    public Task Create(string name, byte[] content);

    public Task<object> Download(string name);

    public void Publish(string name, object data);

    public Task Delete(string key);
}