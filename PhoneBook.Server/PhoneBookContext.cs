using Microsoft.EntityFrameworkCore;
using PhoneBook.Models;

namespace PhoneBook.Server;

public class PhoneBookContext : DbContext
{
    public DbSet<PhoneBookItem> PhoneBook { get; set; }
    
    public PhoneBookContext(DbContextOptions<PhoneBookContext> options) : base(options) 
    { }
}