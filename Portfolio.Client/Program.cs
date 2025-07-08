var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();// Load environment variables from .env file
var apiUrl = Environment.GetEnvironmentVariable("API_URL");
if (string.IsNullOrEmpty(apiUrl))
{
    throw new InvalidOperationException("API_URL environment variable is not set.");
}

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
