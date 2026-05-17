using EventMS.BLL.Interfaces;
using EventMS.BLL.Services;
using EventMS.DAL.Data;
using EventMS.DAL.Interfaces;
using EventMS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Database ──────────────────────────────────────────────────────────
builder.Services.AddDbContext<EventMsdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConn")));

// ── DAL ───────────────────────────────────────────────────────────────
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();

// ── BLL ───────────────────────────────────────────────────────────────
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

// ── MVC ───────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
