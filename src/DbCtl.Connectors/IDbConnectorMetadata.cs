namespace DbCtl.Interfaces
{
    /// <summary>
    /// Provides metadata that is used to select this particular connector.
    /// </summary>
    public interface IDbConnectorMetadata
    {
        /// <summary>
        /// Official name of the connector.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Describes the type of database this connector is intended to work with.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Version of this specific connector.
        /// </summary>
        string Version { get; }
    }
}
