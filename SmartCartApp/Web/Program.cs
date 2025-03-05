using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Repositories.Contracts;
using Web.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);
//connct Mysql database server
// Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

});
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5132, listenOptions =>
    {
        listenOptions.UseHttps(); // Bật HTTPS
    });
});
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.UseAuthorization();

app.MapControllers();

app.Run();
