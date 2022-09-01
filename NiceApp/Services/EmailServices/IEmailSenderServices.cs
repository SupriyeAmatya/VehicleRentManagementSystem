using NiceApp.Models.DataModel;

namespace NiceApp.Services.EmailServices
{
    public interface IEmailSenderServices
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
