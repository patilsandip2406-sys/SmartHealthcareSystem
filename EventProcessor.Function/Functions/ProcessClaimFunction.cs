using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace EventProcessor.Function.Functions
{
    public class ProcessClaimFunction
    {
        private readonly RabbitMQPublisher _rabbitPublisher;


        //public void Run([ServiceBusTrigger("claimsqueue", Connection = "ServiceBusConnection")]string claimMessage, ILogger log)
        //{
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: {claimMessage}");
        //}
        public ProcessClaimFunction()
        {
            // Read settings from environment variables
            var host = Environment.GetEnvironmentVariable("RabbitMQHost");
            var user = Environment.GetEnvironmentVariable("RabbitMQUser");
            var pass = Environment.GetEnvironmentVariable("RabbitMQPassword");
            var queue = Environment.GetEnvironmentVariable("RabbitMQQueue");

            _rabbitPublisher = new RabbitMQPublisher(host, user, pass, queue);
        }

        //[FunctionName("ProcessClaimFunction")]
        //public void Run([RabbitMQTrigger("claims-queue", ConnectionStringSetting = "RabbitMqConnection")] string claimMessage, ILogger log)
        //{
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: {claimMessage}");
        //}


        [FunctionName("ProcessClaimFunction")]
        public void Run([ServiceBusTrigger("claimsqueue", Connection = "ServiceBusConnection")] string serviceBusMessage, ILogger log)
        {
            log.LogInformation($"Received Service Bus message: {serviceBusMessage}");

            // Publish to RabbitMQ
            _rabbitPublisher.PublishMessage(serviceBusMessage);

            log.LogInformation("Message published to RabbitMQ");
        }
    }
}
