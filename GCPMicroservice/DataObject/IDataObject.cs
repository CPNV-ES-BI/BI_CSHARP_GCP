namespace GCPMicroservice;

public interface IDataObject
{
    public Task<bool> DoesExist(string name);

    public void Create(object data);

    public Task<object> Download(string name);

    public void Publish(string name, object data);
}