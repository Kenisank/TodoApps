using App_Core.Dal.Repostories;
using App_Core.Dal.Repostories.Interfaces;
using App_Core.Dal.UnitOfWork;
using App_Core.Data;
using App_Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("TodoConnection")
    ?? throw new InvalidOperationException("Connection string 'TodoConnection' not found.");
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(typeof(Program));



// add UnitOfWork and its interface to the DI container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// add repositories and their interfaces to the DI container
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<TodoContext>();
builder.Services.AddControllersWithViews();




// Logging Configuring
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

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



// Middleware for Logging and Auditing
app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var user = context.User.Identity?.Name ?? "Anonymous";
    var path = context.Request.Path;
    var timestamp = DateTime.UtcNow;

    logger.LogInformation("User: {User}, Path: {Path}, Timestamp: {Timestamp}", user, path, timestamp);

    // Proceed to the next middleware
    await next.Invoke();

    logger.LogInformation("Finished handling request for {Path}", path);
});



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
