using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace SampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person
            {
                Department = "IT",
                Name = "John",
                Surname = "Doe"
            };

            var connFact = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = connFact.CreateConnection())
            using (IModel model = connection.CreateModel())
            {
                model.QueueDeclare(queue: "HR",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(person);

                var body = Encoding.UTF8.GetBytes(message);

                model.BasicPublish(exchange: "",
                       routingKey: "HR",
                       basicProperties: null,
                       body: body);


                Console.WriteLine($"Person Info: {person.Name}  {person.Surname} - {person.Department}");

                BasicConsume.DoConsume();
            }




        }
    }

    internal class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Department { get; set; }
    }
}
