namespace GCPMicroservice;

public interface IDataObject
{
    public Task<bool> DoesExist(string path);

    public void Create(object data);

    public Task<object> Download(string path);

    public void Publish(object data);
}