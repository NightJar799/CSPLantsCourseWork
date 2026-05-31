using System.Security.Claims;

namespace Gardener.entities;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string NickName { get; set; }
    public required string Phone { get; set; }
    public Role Role { get; set; } = Role.ROLE_USER;

    public ClaimsPrincipal ToClaimsPrincipal()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, Id.ToString()),
            new(ClaimTypes.Name, NickName),
            new(ClaimTypes.Email, Login),
            new(ClaimTypes.Role, Role.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Cookie");
        return new ClaimsPrincipal(identity);
    }
}