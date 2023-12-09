using Serilog;
using YamlDotNet.Serialization;

public class Configuration
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? PrivateKey { get; set; }
    public List<string> Hosts { get; set; }
    public string? LogFilePath { get; set; }
    public string? DatabaseUri { get; set; }
    public string? DatabaseName { get; set; }

    public Configuration()
    {
        Hosts = new List<string>();
    }

    public static Configuration Load(string jsonFileName, string hostsFileName)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(jsonFileName);

        var configuration = builder.Build();

        var config = new Configuration();
        configuration.GetSection("Configuration").Bind(config);

        var hostsFilePath = Path.Combine(Directory.GetCurrentDirectory(), hostsFileName);
        config.Hosts = File.ReadAllLines(hostsFilePath).ToList();

        return config;
    }

    public static Configuration FromYaml(string yamlFileName)
    {
        var deserializer = new DeserializerBuilder().Build();
        var yamlFilePath = Path.Combine(Directory.GetCurrentDirectory(), yamlFileName);
        var yaml = File.ReadAllText(yamlFilePath);
        return deserializer.Deserialize<Configuration>(yaml);
    }

    public void Validate()
    {
        if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(PrivateKey))
        {
            Log.Fatal($"Error occurred at {DateTime.Now}, Invalid Configuration: Either Username, Password, or PrivateKey must be provided in the configuration.");
            throw new InvalidOperationException("Either Username, Password, or PrivateKey must be provided in the configuration.");
        }

        // Add other validations as needed
    }
}
