namespace Stockify.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stockify.Models;
using Microsoft.Extensions.Logging;

public class UserRepository : IUserRepository
{
    private readonly StockifyContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(StockifyContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    private string GetTenantService(HttpContext httpContext)
    {
        try
        {
            var user = httpContext.User;
            var serviceTypeClaim = user.FindFirst("Service"); 
            return serviceTypeClaim?.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant service from HttpContext");
            throw;
        }
    }

    private int GetTenantId(HttpContext httpContext)
    {
        try
        {
            var user = httpContext.User;
            var tenantIdClaim = user.FindFirst("TenantId");
            return int.Parse(tenantIdClaim.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant ID from HttpContext");
            throw;
        }
    }

    private bool CanCreateMoreUsers(HttpContext httpContext)
    {
        try
        {
            string serviceType = GetTenantService(httpContext);
            int tenantId = GetTenantId(httpContext);
            int userCount = _context.Users.Count(i => i.TenantId == tenantId);

            return serviceType switch
            {
                Services.Free => userCount < 5,
                Services.Basic => userCount < 20,
                Services.Premium => true,
                _ => false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if more users can be created");
            throw;
        }
    }

    public User Get(int id)
    {
        try
        {
            return _context.Users.FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with id {Id}", id);
            throw;
        }
    }

    public List<User> GetAll()
    {
        try
        {
            return _context.Users.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw;
        }
    }

    public UserDto Add(UserCreateDto userCreateDto, HttpContext httpContext)
    {
        try
        {
            if (!CanCreateMoreUsers(httpContext))
            {
                throw new InvalidOperationException("You have reached the maximum number of users allowed for your service level.");
            }
            var tenant = _context.Tenants.FirstOrDefault(t => t.Name == userCreateDto.TenantName);
            var user = new User
            {
                Name = userCreateDto.Name,
                LastName = userCreateDto.LastName,
                Email = userCreateDto.Email,
                Password = userCreateDto.Password,
                TenantId = tenant.Id,
                Role = "reader"
            };

            
            _context.Users.Add(user);
            SaveChanges(); 

            
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                TenantName = userCreateDto.TenantName,
                TenantId = user.TenantId,
                Role = "reader"
            };

            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding user");
            throw;
        }
    }

    public UserDto MapUserToDto(User user)
    {
        try
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error mapping user to UserDto");
            throw;
        }
    }

    public void Update(User user)
    {
        try
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
               
                existingUser.Name = user.Name;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Role = user.Role;

             
                SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user");
            throw;
        }
    }

    public void Delete(int id)
    {
        try
        {
            var user = _context.Users.Find(id);
            _context.Remove(user);
            SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with id {Id}", id);
            throw;
        }
    }

    public UserDto GetUserFromCredentials(LoginRequest loginRequest)
    {
        try
        {
            var userOut = _context.Users
                .Include(u => u.Tenant)
                .FirstOrDefault(u =>
                    u.Name.ToLower() == loginRequest.Username.ToLower() &&
                    u.Password == loginRequest.Password);

            if (userOut == null)
            {
                return null;
            }

            var userDto = new UserDto
            {
                Id = userOut.Id,
                Name = userOut.Name,
                LastName = userOut.LastName,
                Email = userOut.Email,
                Role = userOut.Role,
                TenantName = userOut.Tenant.Name,
                TenantId = userOut.TenantId
            };

            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user from credentials");
            throw;
        }
    }

    public List<UserDto> GetUsersByTenantId(HttpContext httpContext)
    {
        try
        {
            var user = httpContext.User;
            var tenantIdClaim = user.FindFirst("TenantId");
            int tenantId = Convert.ToInt32(tenantIdClaim.Value);
            var userDtos = _context.Users.Where(u => u.TenantId == tenantId).Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role
            }).ToList();

            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users by tenant id");
            throw;
        }
    }

    public void SaveChanges()
    {
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving changes");
            throw;
        }
    }
}
