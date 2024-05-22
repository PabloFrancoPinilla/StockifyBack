using Stockify.Models;

namespace Stockify.Data;

public interface IUserRepository{
    List<User> GetAll();
    User Get(int id);
    UserDto Add (UserCreateDto userCreateDto);
    void Update (User User);
    void Delete (int id);
    UserDto GetUserFromCredentials(UserLogin userLogin);
    UserDto MapUserToDto(User user);
    List<UserDto> GetUsersByTenantId (int tenantId);
}
