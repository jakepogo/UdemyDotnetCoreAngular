using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        // Interfaces are easy to test and can be instantiated
        string CreateToken(AppUser user);
    }
}