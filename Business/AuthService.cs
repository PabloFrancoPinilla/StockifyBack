namespace Stockify.Business
{
    using Stockify.Data;
    using Stockify.Models;
    using System;

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly IUserService _service;

        public AuthService(IUserRepository repository, IUserService service)
        {
            _repository = repository;
            _service = service;
        }

        public UserDto Login(UserLogin userLogin)
        {
            var user = _repository.GetUserFromCredentials(userLogin);
            if (user == null)
            {
                throw new Exception("Invalid login credentials.");
            }
            return user;
        }

        public UserDto Register(UserCreateDto userCreateDto)
        {
            var userout = _service.Add(userCreateDto);
            return userout;
        }
    }
}
