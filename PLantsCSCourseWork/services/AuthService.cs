using Gardener.Database;
using Gardener.dto.request;
using Gardener.entities;
using Gardener.Utils;
using Microsoft.EntityFrameworkCore;

namespace Gardener.Services;

public class AuthService(GardenerDbContext db) : IAuthService
{
    public async Task<User?> RegisterAsync(RegisterRequest request)
    {
        if (await db.Users.AnyAsync(u => u.Login == request.Email))
            return null;
        if (await db.Users.AnyAsync(u => u.Phone == request.Phone))
            return null;

        var user = new User
        {
            Login = request.Email,
            Password = PasswordHelper.HashPassword(request.Password),
            NickName = string.IsNullOrWhiteSpace(request.Nickname) ? request.Email.Split('@')[0] : request.Nickname,
            Phone = request.Phone,
            Role = Role.ROLE_USER
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        db.Preferences.Add(new Preferences { Id = user.Id, User = user });
        await db.SaveChangesAsync();

        return user;
    }

    public async Task<User?> LoginAsync(LoginRequest request)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Login == request.Email);
        if (user == null) return null;
        return PasswordHelper.VerifyPassword(request.Password, user.Password) ? user : null;
    }
}