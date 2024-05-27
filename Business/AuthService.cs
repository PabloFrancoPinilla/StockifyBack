namespace Stockify.Business
{
    using Stockify.Data;
    using Stockify.Models;
    using System;

    public class AuthService: IAuthService 
    {
        private readonly IUserRepository _repository;
        private readonly IUserService _service;
        private readonly ITenantService _tenantService;

        public AuthService(IUserRepository repository, IUserService service, ITenantService tenantService)
        {
            _repository = repository;
            _service = service;
            _tenantService = tenantService;
        }

        public object Login(LoginRequest loginRequest)
        {
            if (loginRequest.IsTenant)
            {
                var tenant = _tenantService.GetTenantFromCredentials(loginRequest);
                if (tenant == null)
                {
                    throw new Exception("Invalid login credentials.");
                }
                return tenant;
            }
            else
            {
                var user = _repository.GetUserFromCredentials(loginRequest);
                if (user == null)
                {
                    throw new Exception("Invalid login credentials.");
                }
                return user;
            }
        }

          public UserDto Register(UserCreateDto userCreateDto)
        {
            var userout = _service.Add(userCreateDto);
            return userout;
        }
    }
}
