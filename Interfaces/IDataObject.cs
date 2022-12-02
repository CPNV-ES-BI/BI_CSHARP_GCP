namespace GCPMicroservice;

public interface IDataObject
{
    public bool DoesExist(string path);

    public void Create(object data);

    public object Download(string path);

    public void Publish(object data);
}