namespace Stockify.Data;
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
    public void Add(User User)
    {
        _context.Users.Add(User);
        SaveChanges();
    }
    public void Update (User User){
        
    }
    public void Delete (int id){
        var User = _context.Users.Find(id);
        _context.Remove(User);
        SaveChanges(); 
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
