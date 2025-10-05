using System;

public class RabbitMQPublisher
{
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private readonly string _queueName;

    public RabbitMQPublisher(string hostName, string userName, string password, string queueName)
    {
        _hostName = hostName;
        _userName = userName;
        _password = password;
        _queueName = queueName;
    }

    public void PublishMessage(string message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        Console.WriteLine($"Sent: {message}");
    }
}
