namespace DbCtl.Interfaces
{
    public interface IDbConnectorMetadata
    {
        string Name { get; }
        string Description { get; }
        string Version { get; }
    }
}
