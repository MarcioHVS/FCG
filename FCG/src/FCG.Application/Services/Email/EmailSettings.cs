namespace FCG.Application.Services.Email
{
    public class EmailSettings
    {
        public required string SmtpServer { get; set; }
        public required int SmtpPort { get; set; }
        public required string SmtpUser { get; set; }
        public required string SmtpPassword { get; set; }
    }
}
