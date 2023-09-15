namespace DropMate2.Application.ServiceContracts
{
    public interface IEmailService
    {
        void SendEmail(string toAddress, string subject, string body);
    }
}
