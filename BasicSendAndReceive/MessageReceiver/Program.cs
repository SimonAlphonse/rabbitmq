using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageReceiver
{
    static class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    string queueName = "demo-queue";

                    channel.QueueDeclare(
                        queue: queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        byte[] messageBytes = e.Body.ToArray();
                        string message = Encoding.UTF8.GetString(messageBytes);
                        Console.WriteLine($"Message received at {DateTime.Now:MMM-dd hh:mm:ss.ff} - {message}");
                    };

                    channel.BasicConsume(
                        queueName,
                        autoAck: true,
                        consumer);

                    Console.WriteLine($"Receiver is ready...{Environment.NewLine}");
                    Console.ReadKey();
                }
            }

        }
    }
}