using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using MyProClip.Hubs;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Services;
using MyProClip_DAL.Data;
using MyProClip_DAL.Repositories;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 3, 0)),
        mySqlOptions => mySqlOptions.MigrationsAssembly("MyProClip")
    ));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IClipService, ClipService>();
builder.Services.AddScoped<IClipRepository, ClipRepository>();

builder.Services.AddScoped<IFriendshipService, FriendshipService>();
builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var thumbnailsProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Thumbnails"));
var videosProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Videos"));

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = thumbnailsProvider,
    RequestPath = "/Thumbnails"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = videosProvider,
    RequestPath = "/Videos"
});

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/ChatHub");

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
