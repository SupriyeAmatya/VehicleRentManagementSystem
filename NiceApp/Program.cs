using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using NiceApp.CustomEmailConfirmationTokenProvider;
using NiceApp.Data;
using NiceApp.Models.DataModel;
using NiceApp.Services.EmailServices;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;
var emailConfig = Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.AddControllers();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserAndAdmin", policy => policy.RequireRole("Admin").RequireRole("User"));
});
builder.Services.AddDbContext<AppDbContext>(e =>
    e.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 4;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
    //config.SignIn.RequireConfirmedAccount = true;
    config.SignIn.RequireConfirmedEmail = true;
    config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    config.Tokens.ProviderMap.Add("CustomEmailConfirmation",
         new TokenProviderDescriptor(
             typeof(CustomEmailConfirmationTokenProvider<AppUser>)));
    config.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
    config.Tokens.PasswordResetTokenProvider = "CustomEmailConfirmation";
}).AddEntityFrameworkStores<AppDbContext>()
    //.AddDefaultTokenProviders()
    ;
//builder.Services.AddScoped<CustomEmailConfirmationTokenProvider<AppUser>>();
builder.Services.AddTransient<CustomEmailConfirmationTokenProvider<AppUser>>();

builder.Services.AddTransient<IEmailSenderServices, EmailSenderServices>();

builder.Services.AddMailKit(config =>
{
    var options = new MailKitOptions();
    config.UseMailKit(options);
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(config =>
       config.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Identity.Cookie";
    config.LoginPath = "/Account/Login";
    config.ExpireTimeSpan = TimeSpan.FromDays(5);
    config.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Userhome}/{action=Index}/{id?}");

app.Run();
