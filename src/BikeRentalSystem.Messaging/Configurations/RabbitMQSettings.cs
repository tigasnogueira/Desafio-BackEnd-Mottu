﻿namespace BikeRentalSystem.Messaging.Configurations;

public class RabbitMQSettings
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Exchange { get; set; }
    public string Queue { get; set; }
    public string RoutingKey { get; set; }
}
