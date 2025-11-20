using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/app-log-.txt",
                  rollingInterval: RollingInterval.Day,
                  retainedFileCountLimit: 15,
                  shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

var allowedOrigins = "_allowedOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowedOrigins, policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:4200",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";    // Redirect if not authenticated
        options.LogoutPath = "/Auth/Logout";  // Redirect on logout
        options.Cookie.HttpOnly = true;       // JavaScript cannot read it
        options.Cookie.SameSite = SameSiteMode.None; // ? required for cross-origin
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // ? must be HTTPS
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;     // Refresh expiration on activity
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<EventTrackerLibrary.EventTrackerRepository>();
builder.Services.AddScoped<EventTrackerAPI.Services.Intefaces.IUserService, EventTrackerAPI.Services.UserService>();
builder.Services.AddScoped<EventTrackerAPI.Services.Intefaces.ITaskService, EventTrackerAPI.Services.TaskService>();
builder.Services.AddScoped<EventTrackerAPI.Services.Intefaces.IAuthService, EventTrackerAPI.Services.AuthService>();
builder.Services.AddScoped<EventTrackerAPI.Services.Intefaces.ITaskCommentService, EventTrackerAPI.Services.TaskCommentService>();

var app = builder.Build();

app.UseExceptionHandler("/error");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(allowedOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

Log.CloseAndFlush();
