namespace GimnasioFit.Core.Services
{
    public interface ITokenService
    {
        string GenerarToken(int id, string nombre, string email, string rol, string? nivelAcceso = null);
    }
}