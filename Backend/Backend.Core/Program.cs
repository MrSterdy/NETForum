using System.Net;

using Backend.Core.Database;
using Backend.Core.Database.Repositories;
using Backend.Core.Filters;
using Backend.Core.Identity;
using Backend.Core.Mail;
using Backend.Core.Middlewares;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
);

builder.Services.AddCors(options => 
        options.AddPolicy("CORS", b => 
            b
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(builder.Configuration["AllowedOrigins"]!.Split(";"))
        )
);

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;

        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole<int>>()
    .AddSignInManager<SignInManager>()
    .AddEntityFrameworkStores<Context>();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.Cookie.Name = "NETForumIdentity";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;

        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            
            return Task.CompletedTask;
        };
    });

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddScoped<IThreadRepository, ThreadRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddControllers(options => options.Filters.Add<DisabledUserFilter>());

var app = builder.Build();

app.UseCors("CORS");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.InitializeDatabase();

app.Run();

public partial class Program { }