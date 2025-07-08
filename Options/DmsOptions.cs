namespace portal.Options;

public class SftpOptions
{
    public string Host { get; set; } = "";
    public int Port { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LocalFileStorageOptions
{
    public string BasePath { get; set; } = "srv/uploads/ftp-service/erp/documents";
}
