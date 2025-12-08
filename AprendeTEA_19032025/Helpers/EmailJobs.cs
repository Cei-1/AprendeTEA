namespace AprendeTEA_19032025.Helpers
{
    public class EmailJobs
    {
        private readonly IEmailSender _emailSender;

        public EmailJobs(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        // Este método lo llama Hangfire
        public Task EnviarConfirmacion(string email, int idUsuario, string token)
        {
            return _emailSender.EnviarConfirmacionEmailAsync(email, idUsuario, token);
        }
    }
}
