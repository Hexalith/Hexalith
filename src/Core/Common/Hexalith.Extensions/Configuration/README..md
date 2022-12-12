# Hexalith.Extensions.Configuration

ISettings is the interface for application settings. It defines the configuration section name.

```csharp
public interface ISettings
{
  static abstract string ConfigurationName();
}
```

Example:

```csharp
public class MySettings : ISettings
{
  public string? FirstName { get; set; }
	
  public string? LastName { get; set; }

  public static string ConfigurationName() => "Me";	
}
```

appsettings.json:

```json
{
  "Me": {
    "FirstName": "John",
	"LastName": "Doe"
  }
}
```

Configure settings in the services collection:

```csharp
  services.ConfigureSettings<MySettings>(configuration);
```

Use settings in a service:

```csharp
public class MyService
{
  private readonly IOptions<MySettings> _settings;

  public MyService(IOptions<MySettings> settings)
  {
    _settings = settings;
  }

  public void DoSomething()
  {
    Console.WriteLine($"Hello {_settings.Value.FirstName} {_settings.Value.LastName}!");
  }
}
```
