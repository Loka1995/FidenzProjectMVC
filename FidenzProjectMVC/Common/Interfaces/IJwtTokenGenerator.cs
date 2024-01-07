namespace FidenzProjectMVC.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateJwtToken(string username, string role);
    }
}
