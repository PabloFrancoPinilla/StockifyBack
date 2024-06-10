namespace Stockify.Business;

using Microsoft.AspNetCore.Http;
using Stockify.Data;
using Stockify.Models;

public interface IAuthService
{
    Object Login(LoginRequest credentials);
    UserDto Register(UserCreateDto userCreateDto, HttpContext httpContext);
}