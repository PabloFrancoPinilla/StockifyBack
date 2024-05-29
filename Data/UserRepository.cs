namespace Stockify.Data;

using Microsoft.EntityFrameworkCore;
using Stockify.Models;

public class UserRepository : IUserRepository
{
    private readonly StockifyContext _context;

    public UserRepository(StockifyContext context)
    {
        _context = context;
    }
    public User Get(int id)
    {
        return _context.Users.FirstOrDefault(p => p.Id == id);
    }
    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }
    public UserDto Add(UserCreateDto userCreateDto)
    {
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
        var userOut = _context.Users.Include(u => u.Tenant).FirstOrDefault(u => u.Name == loginRequest.Username && u.Password == loginRequest.Password);
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
    public List<UserDto> GetUsersByTenantId(int tenantId)
    {
        var user = _context.Users.Where(u => u.TenantId == tenantId).Select(u => new UserDto
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
        return user;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
