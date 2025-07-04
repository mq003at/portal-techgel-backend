namespace portal.Configurations.CAP;

public class RabbitMQSettings
{
    public string HostName { get; set; } = default!;
    public string VirtualHost { get; set; } = "/";
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int Port { get; set; } = 5672;
    public bool UseSsl { get; set; } = false;
}
