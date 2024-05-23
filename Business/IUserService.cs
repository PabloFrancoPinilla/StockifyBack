namespace Stockify.Business;
using Stockify.Models;

public interface IUserService {
    UserDto Add(UserCreateDto userCreateDto);
    void Delete(int id);
    void Update(User User);
    User Get (int id);
    List<User> GetAll();
    UserDto GetUserFromCredentials(LoginRequest loginRequest);
    List<UserDto> GetUserByTenantId(int tenantId);
    
    
}