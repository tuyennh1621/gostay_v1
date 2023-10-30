using GoStay.Web.Model;

namespace GoStay.Web.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, IdentityUser userModel);
    }
}
