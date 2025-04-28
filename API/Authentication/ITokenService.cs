namespace API.Authentication
{
    public interface ITokenService
    {
        string CreateAccessToken(string email, int customerId);
        string CreateRefreshToken();
    }
}
