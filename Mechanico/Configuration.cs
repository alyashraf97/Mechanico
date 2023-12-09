using YamlDotNet.Serialization;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.PortableExecutable;

public class Configuration
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? PrivateKey { get; set; }
    public Command[]? EnabledCommands { get; set; }
    public Machine[] Hosts { get; set; }
    public string LogFilePath { get; set; }
    public string DatabaseUri { get; set; }
    public string DatabaseName { get; set; }

    public Configuration(string yamlFileName)
    {
        var deserializer = new DeserializerBuilder().Build();
        var yamlFilePath = Path.Combine(Directory.GetCurrentDirectory(), yamlFileName);
        var yaml = File.ReadAllText(yamlFilePath);
        var config = deserializer.Deserialize<Configuration>(yaml);

        // Copy properties from config to this instance
        Username = config.Username;
        Password = config.Password;
        PrivateKey = config.PrivateKey;
        EnabledCommands = config.EnabledCommands;
        Hosts = config.Hosts;
        LogFilePath = config.LogFilePath;
        DatabaseUri = config.DatabaseUri;
        DatabaseName = config.DatabaseName;
    }


    public static Configuration FromYaml(string yamlFileName)
    {
        var deserializer = new DeserializerBuilder().Build();
        var yamlFilePath = Path.Combine(Directory.GetCurrentDirectory(), yamlFileName);
        var yaml = File.ReadAllText(yamlFilePath);
        return deserializer.Deserialize<Configuration>(yaml);
    }
}
