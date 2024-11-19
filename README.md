# channel-logging
It is a customized Logging library to push log messages to the channel

[//]: # (Special thanks to git@github.com:poychang)

## How to implement
**Step 1: Add some setting values in appsettings.json**
```csharp
"ChannelLogging": {
    "EnableLogging": false,
    "WebhookUrl": "MSteams webhook",
    "LogLevel": "Error"
}
```

**Step 2: Load setting values to ChannelLoggingSetting model**
```csharp
builder.Services.Configure<ChannelLoggingSetting>(builder.Configuration.GetSection("ChannelLogging"));
```

**Step 3: Register DI LoggerFactoryBuilder**
```csharp
builder.Services.AddScoped<LoggerFactoryBuilder>();
```

**Step 4: User LoggerFactoryBuilder in target class constructor**
```csharp
private readonly ILogger<TestController> logger;
public TestController(LoggerFactoryBuilder loggerFactoryBuilder)
{
    this.logger = loggerFactoryBuilder.Build().CreateLogger<TestController>();
}
```
And then you can you logger normally
```csharp
[HttpGet("logging-webhook")]
public IActionResult TestLoggingWebHook()
{
    try
    {
        logger.LogInformation("Testing channel webhook - Information");
        logger.LogWarning("Testing channel webhook - Warning");
        logger.LogError("Testing channel webhook - Error");
        return this.Ok($"Set master data status code 200");
    }
    catch (Exception e)
    {
        this.logger.LogError("Error " + e);
        return this.BadRequest($"Error {e.Message}");
    }
}
```