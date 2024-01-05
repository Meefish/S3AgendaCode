using Microsoft.EntityFrameworkCore;
using Smart_Agenda_API;
using Smart_Agenda_API.Configuration;
using Smart_Agenda_DAL;
using Smart_Agenda_Logic.Interfaces;
using Smart_Agenda_Logic.Managers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddCors();


builder.Services.AddScoped<IUserDAL, UserDAL>();
builder.Services.AddScoped<ITaskDAL, TaskDAL>();
builder.Services.AddScoped<ICalendarDAL, CalendarDAL>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<TaskManager>();
builder.Services.AddScoped<CalendarManager>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSingleton<WebSocketConfiguration>();
builder.Services.AddScoped<WebSocketHandler>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var environment = builder.Environment;
var connectionStringName = environment.IsEnvironment("Testing") ? "TestConnection" : "DefaultConnection";
var connectionString = builder.Configuration.GetConnectionString(connectionStringName);


builder.Services.AddDbContext<Smart_Agenda_DAL.DataBase>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

var webSocketConfig = app.Services.GetRequiredService<WebSocketConfiguration>();
webSocketConfig.ConfigureWebSockets(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(',') ?? Array.Empty<string>();


app.UseCors(builder =>
    builder.WithOrigins(allowedOrigins)
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }