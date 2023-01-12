using Backend.Core.Database;
using Backend.Core.Database.Repositories;
using Backend.Core.Mail;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options => 
        options.AddPolicy("CORS", b => 
            b
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(builder.Configuration["AllowedOrigins"]!.Split(";"))
        )
);

builder.Services.AddIdentityCore<IdentityUser<int>>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;

        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "NETForumLogin";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        
        options.SlidingExpiration = true;
        
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddScoped<IThreadRepository, ThreadRepository>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("CORS");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }