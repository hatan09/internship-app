using Models;

namespace Services;

public interface IEmailService
{
    string GetTemplate(string type);
    Task Send(EmailModel emailModel);
}