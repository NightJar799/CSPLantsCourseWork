using Gardener.dto.request;
using Gardener.entities;

namespace Gardener.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(RegisterRequest request);
    Task<User?> LoginAsync(LoginRequest request);
}