namespace Stockify.Business;

using Microsoft.AspNetCore.Http;
using Stockify.Models;

public interface IUserService
{
    UserDto Add(UserCreateDto userCreateDto, HttpContext httpContext);
    void Delete(int id);
    void Update(User User);
    User Get(int id);
    List<User> GetAll();
    UserDto GetUserFromCredentials(LoginRequest loginRequest);
    List<UserDto> GetUsersByTenantId(HttpContext httpContext);


}