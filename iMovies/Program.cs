using System.Security.Claims;
using System.Text;
using iMovies.Data;
using iMovies.Data.Services;
using iMovies.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});





builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Set the minimum length requirement for passwords
    options.Password.RequiredLength = 8;
    // Disable other password complexity requirements
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllers();



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireRole("User"));
});

builder.Services.AddScoped<ITokenService, TokenService>();







builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigninKey"])),
        RoleClaimType = ClaimTypes.Role // Ensure this matches the role claim in your token

    };
})
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true; // Allow JavaScript access to the cookie
    options.Cookie.SameSite = SameSiteMode.Lax; // Allow cross-site requests
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .WithOrigins("http://localhost:5238") // Make sure this is the correct frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // This must be enabled to allow cookies
    });
});


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


app.UseCors("CorsPolicy");

app.UseAuthentication();  // Should come before UseAuthorization
app.UseAuthorization();   // Authorization should come after authentication






app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.MapControllers(); // This line ensures the API routes work



app.Run();
