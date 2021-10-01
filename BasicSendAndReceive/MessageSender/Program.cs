using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MessageSender
{
    static class Program
    {
        static void Main(string[] args)
        {
            do
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri("amqp://guest:guest@localhost:5672")
                };

                using (IConnection connection = factory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        string queueName = "simon-queue";

                        channel.QueueDeclare(
                            queue: queueName,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        var message = new { Name = "Simon", Message = $"Message created at {DateTime.Now:MMM-dd hh:mm:ss.ff}" };
                        byte[] messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                        Console.WriteLine(message.Message);
                        channel.BasicPublish(string.Empty, queueName, null, messageBytes);
                    }
                }
            }
            while (Console.ReadKey().Key == ConsoleKey.Enter);
        }
    }
}