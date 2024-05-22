namespace Stockify.Business;
using Stockify.Data;
using Stockify.Models;
public class UserService : IUserService
{
    private readonly IUserRepository _UserRepository;

    public UserService(IUserRepository UserRepository)
    {
        _UserRepository = UserRepository;
    }
    public User Get(int id) => _UserRepository.Get(id);
    public void Update(User User) => _UserRepository.Update(User);
    public void Delete(int id) => _UserRepository.Delete(id);
    public List<User> GetAll() => _UserRepository.GetAll();
    public UserDto Add(UserCreateDto userCreateDto) => _UserRepository.Add(userCreateDto);
    public UserDto GetUserFromCredentials(UserLogin userLogin) => _UserRepository.GetUserFromCredentials(userLogin);
    public List<UserDto> GetUserByTenantId(int tenantId) => _UserRepository.GetUsersByTenantId(tenantId);



}
