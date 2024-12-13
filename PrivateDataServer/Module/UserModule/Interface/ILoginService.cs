namespace PrivateDataServer.Module.UserModule.Interface;
using PrivateDataServer.Module.Request;
using PrivateDataServer.Module.Response;

public interface ILoginService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}