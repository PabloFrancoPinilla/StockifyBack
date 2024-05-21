namespace Stockify.Business;

using Stockify.Data;
using Stockify.Models;

public interface IAuthService {
    UserDto Login (UserLogin credentials);
    UserDto Register (UserCreateDto userCreateDto);
}