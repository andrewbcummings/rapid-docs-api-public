using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using rapid_docs_core.Authentication;
using rapid_docs_models.DataAccess;

namespace rapid_docs_services.Services.Email
{
    public class EmailService : BaseService, IEmailService
    {
        //private SmtpClient _smtpClient;
        private EmailOptions _emailOptions;
        public EmailService( VidaDocsDbContext dbContext, 
            IMapper mapper, VidaDocsContext ctx, IOptions<EmailOptions> emailOptions) : base(dbContext, mapper,ctx)
        {
           //_smtpClient = smtpClient;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.ctx = ctx;
            this._emailOptions = emailOptions.Value;
        }

        public Task<bool> SendEmailAsync(string to, string subject, string message)
        {
            // create message
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(this._emailOptions.SenderEmail);
            if (!string.IsNullOrEmpty(this._emailOptions.SenderName))
                email.Sender.Name = this._emailOptions.SenderName;
            email.From.Add(email.Sender);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            // send email
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(this._emailOptions.Address, this._emailOptions.Port, this._emailOptions.SecureSocketOptions);
                smtp.Authenticate(this._emailOptions.Username, this._emailOptions.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            return Task.FromResult(true);
        }
    }
}