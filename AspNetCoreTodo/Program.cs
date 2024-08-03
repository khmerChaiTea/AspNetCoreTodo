using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Services;
using AspNetCoreTodo;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Add services.AddSingleton<ITodoItemService, FakeTodoItemService>(); but make a little change
// Make sure to have using AspNetCoreTodo.Services; p. 39, p. 59
builder.Services.AddScoped<ITodoItemService, TodoItemService>();

var app = builder.Build();

// Called the method p.92
InitializeDatabase(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// We no longer use IWebHost host; we use WebApplication app
// using Microsoft.Extensions.DependencyInjection;
static void InitializeDatabase(WebApplication app)
{
	// use app instead of host p.92
	using (var scope = app.Services.CreateScope())
	{
		var services = scope.ServiceProvider;

		// Use try and catch sparingly
		try
		{
			SeedData.InitializeAsync(services).Wait();
		}
		catch (Exception ex)
		{
			var logger = services.GetRequiredService<ILogger<Program>>();
			logger.LogError(ex, "Error occurred while seeding database.");
		}
		finally
		{
			// Always run
		}
	}
}

public partial class Program { }