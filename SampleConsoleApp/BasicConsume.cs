using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleConsoleApp
{
    public static class BasicConsume
    {
        public static void DoConsume()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "HR",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.Span;
                    var message = Encoding.UTF8.GetString(body);
                    Person person = JsonConvert.DeserializeObject<Person>(message);
                    Console.WriteLine($" Mr.  {person.Name} {person.Surname} - {person.Department}");
                };
                channel.BasicConsume(queue: "HR", 
                    noLocal: true,
                    consumer: consumer);

                Console.WriteLine(" You are hired !!! ");
            }
        }
    }
}
internal class Person
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Department { get; set; }
}