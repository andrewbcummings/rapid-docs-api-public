namespace rapid_docs_core.ApplicationSettings
{
    public class BlobStorageSettings
    {
        public string Protocol { get; set; }
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string EndpointSuffix { get; set; }
        public string ContainerName { get; set; }
        public string PublicContainerName { get; set; }
    }
}
