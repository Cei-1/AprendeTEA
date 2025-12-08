namespace AprendeTEA_19032025.Helpers
{
    public interface IEmailSender
    {
        Task EnviarConfirmacionEmailAsync(string email, int idUsuario, string token);
        // Podrías agregar más métodos: EnviarRecuperacionPasswordAsync, etc.
    }
}
