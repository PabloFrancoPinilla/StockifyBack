namespace Stockify.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stockify.Models;

public class UserRepository : IUserRepository
{
    private readonly StockifyContext _context;

    public UserRepository(StockifyContext context)
    {
        _context = context;
    }
    private string GetTenantService(HttpContext httpContext)
    {

        var user = httpContext.User;
        var serviceTypeClaim = user.FindFirst("Service"); // Suponiendo que el token incluye este claim
        return serviceTypeClaim?.Value;
    }
    private int GetTenantId(HttpContext httpContext)
    {
        var user = httpContext.User;
        var tenantIdClaim = user.FindFirst("TenantId");
        return int.Parse(tenantIdClaim.Value);
    }
    private bool CanCreateMoreUsers(HttpContext httpContext)
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
    public User Get(int id)
    {
        return _context.Users.FirstOrDefault(p => p.Id == id);
    }
    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }
    public UserDto Add(UserCreateDto userCreateDto, HttpContext httpContext)
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

        // Agregar el usuario a la base de datos
        _context.Users.Add(user);
        SaveChanges(); // Asumiendo que SaveChanges() guarda los cambios en la base de datos

        // Mapear User a UserDto
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            TenantName = userCreateDto.TenantName,
            TenantId = user.TenantId,
            Role = "reader"

            // Asignar el Role si es necesario
        };

        return userDto;
    }
    public UserDto MapUserToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,

        };
    }
    public void Update(User user)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            // Actualizar las propiedades del usuario existente
            existingUser.Name = user.Name;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Role = user.Role;

            // Guardar los cambios en la base de datos
            SaveChanges();
        }
    }

    public void Delete(int id)
    {
        var User = _context.Users.Find(id);
        _context.Remove(User);
        SaveChanges();
    }
    public UserDto GetUserFromCredentials(LoginRequest loginRequest)
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

        Console.WriteLine(userDto.Name);
        return userDto;
    }
    public List<UserDto> GetUsersByTenantId(HttpContext httpContext)
    {
        var user = httpContext.User;
        var tenantIdClaim = user.FindFirst("TenantId");
        int tenantId = Convert.ToInt32(tenantIdClaim.Value);
        var userdto = _context.Users.Where(u => u.TenantId == tenantId).Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            LastName = u.LastName,
            Email = u.Email,
            Role = u.Role

        }).ToList();
        if (user == null)
        {
            return null;
        }
        return userdto;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
