namespace User.Api.Configurations
{
    public record MongoDbConfigurations
    {
        public string ConnectionString { get; init; } = string.Empty;
        public string DatabaseName { get; init; } = string.Empty;
    }
}
