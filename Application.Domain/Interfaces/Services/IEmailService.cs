using Core.Application.Dtos.Email;

namespace Core.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(EmailRequest request);
    }
}
