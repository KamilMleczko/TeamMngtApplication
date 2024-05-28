using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeamMngt.Data;

using System.Globalization;


var cultureInfo = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TeamMngtContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TeamMngtContext") ?? throw new InvalidOperationException("Connection string 'TeamMngtContext' not found.")));



// Add services to the container.
builder.Services.AddControllersWithViews();

//Dodanie obsługo sesji
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(180); 
    options.Cookie.HttpOnly = true;//plik cookie jest niedostępny przez skrypt po stronie klienta
    options.Cookie.IsEssential = true;//pliki cookie sesji będą zapisywane dzięki czemu sesje będzie mogła być śledzona podczas nawigacji lub przeładowania strony
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

app.UseAuthorization();
//Dodanie obsługo sesji
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Use(async (ctx, next) =>
{
    if (ctx.Session.GetString("nazwa") == null  &&
        !ctx.Request.Path.Value.StartsWith("/User/WczytajFormularz") &&
        !ctx.Request.Path.Value.Equals("/") &&
        !ctx.Request.Path.Value.StartsWith("/Home"))
    {
        ctx.Response.Redirect("/User/WczytajFormularz");
    }
    else
    {
        await next();
    }
});

app.Run();