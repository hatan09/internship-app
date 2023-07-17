using Models;

namespace InternshipApp.Services;

public interface IEmailService
{
    string GetTemplate(string type);
    Task Send(EmailModel emailModel);
}