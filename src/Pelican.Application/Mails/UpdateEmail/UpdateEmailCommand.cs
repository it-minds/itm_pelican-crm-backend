using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Mails.UpdateEmail;

public record UpdateEmailCommand(EmailDto Email) : ICommand<EmailDto>;
