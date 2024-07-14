﻿using BikeRentalSystem.Messaging.Events;
using BikeRentalSystem.Messaging.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BikeRentalSystem.Messaging.Consumers;

public class MotorcycleRegisteredConsumer : IMessageConsumer
{
    private readonly IModel _channel;
    private readonly string _queueName = "rental_queue";

    public MotorcycleRegisteredConsumer(IModel channel)
    {
        _channel = channel;
    }

    public void Consume()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var motorcycleRegisteredEvent = JsonConvert.DeserializeObject<MotorcycleRegistered>(message);
        };

        _channel.BasicConsume(_queueName, true, consumer);
    }
}
