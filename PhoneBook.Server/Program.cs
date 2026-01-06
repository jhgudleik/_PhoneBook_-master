using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneBook.Models;
using PhoneBook.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PhoneBookContext>(options => options.UseNpgsql(connection));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/phonebook", async (PhoneBookContext context) => 
    await context.PhoneBook.ToListAsync());
app.MapPost("/phonebook", async (PhoneBookContext context, PhoneBookItem item) => 
{
    var newPhoneBookItem = await context.PhoneBook.AddAsync(item);
    await context.SaveChangesAsync();
    
    return newPhoneBookItem.Entity;
});

// Автоматическое применение миграций при старте
/*
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PhoneBookContext>();
    db.Database.Migrate(); // Создаст базу и таблицы если их нет
}
*/
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PhoneBookContext>();
    db.Database.EnsureCreated(); // Создаст таблицы без миграций
}

await app.RunAsync();