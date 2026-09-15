using BecomeAWizzard.Api.Data;
using BecomeAWizzard.Api.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AppDbContext depends on the SQLite connection string in appsettings.json.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<GameService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "become-a-wizzard.auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(options => options.AddPolicy("Vite", policy =>
    policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()));

var app = builder.Build();

Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "Data"));

// SeedData depends on AppDbContext and creates local demo content after the schema exists.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    await SeedData.InitializeAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Vite");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// The production-style local host depends on `npm run build`, which creates client/dist.
// During development Vite serves the same React files through http://localhost:5173.
var clientDist = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "..", "client", "dist"));
if (Directory.Exists(clientDist))
{
    var files = new PhysicalFileProvider(clientDist);
    app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = files });
    app.UseStaticFiles(new StaticFileOptions { FileProvider = files });
    app.MapFallback(async context => await context.Response.SendFileAsync(Path.Combine(clientDist, "index.html")));
}
app.Run();
