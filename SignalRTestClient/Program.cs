// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Hello, World!");


var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:5004/notifications", options =>
    {
        options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets |
                             Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
    })
    .WithAutomaticReconnect()
    .Build();

Console.WriteLine("After, Connections!");

// Receive messages from hub
connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    Console.WriteLine($"{user}: {message}");
});

Console.WriteLine("After, Connection.on!");

connection.Closed += async (error) =>
{
    Console.WriteLine("Connection closed!");
    if (error != null) Console.WriteLine($"Error: {error.Message}");
    await Task.Delay(1000);
    await connection.StartAsync();
};

Console.WriteLine("After, Connections closed");

try
{
    Console.WriteLine("Before, StartAsync");
    await connection.StartAsync();
    Console.WriteLine("Connected to Notification Hub!");
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
    return;
}

// Send a test message
await connection.InvokeAsync("SendMessage", "Hello from console client!");
Console.WriteLine("Test message sent. Press any key to exit.");
Console.ReadKey();
