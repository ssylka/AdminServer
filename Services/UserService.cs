namespace WebServer.Services
{
    public class UserService
    {
        private readonly EmailService emailService;

        public UserService(EmailService emailService)
        {
            this.emailService = emailService;
        }
    }
}
