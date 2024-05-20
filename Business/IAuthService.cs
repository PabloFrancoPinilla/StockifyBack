namespace Stockify.Business;

using Stockify.Data;
using Stockify.Models;

public interface IAuthService {
    string Login (UserLogin credentials);
    string Register (UserCreateDto userCreateDto);
}