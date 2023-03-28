using MailKit.Security;

// Reference: https://www.ryadel.com/en/asp-net-core-send-email-messages-smtp-mailkit/
public class EmailOptions
{
    public EmailOptions()
    {
        SecureSocketOptions = SecureSocketOptions.Auto;
    }

    public string Address { get; set; }

    public int Port { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public SecureSocketOptions SecureSocketOptions { get; set; }

    public string SenderEmail { get; set; }

    public string SenderName { get; set; }
}