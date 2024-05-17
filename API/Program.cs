using Stockify.Data;
using Stockify.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
/* Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
    .WriteTo.File("logs/errorlogs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(dispose: true); */
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*  builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        }); */

var connectionString = builder.Configuration.GetConnectionString("ServerDB");
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IProductService, ProductService>(); 

// Register database context
builder.Services.AddDbContext<StockifyContext>(options =>
    options.UseSqlServer(connectionString));

// Register repositories
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register services
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

/* app.UseHttpsRedirection();*/
app.UseAuthorization();
app.UseCors(options =>
{
    options.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});
app.MapControllers();

app.Run();

