using crudSegnalR.Infrastructure.Identity;
using crudSegnalR.Infrastructure.Identity.Service;
using crudSegnalR.Infrastructure.Shared;
using crudSignalR.Core.Application.Interface.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddLayerIdenityWeb(builder.Configuration);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());//builder.Services.AddTransient<IAccountService, AccountService>();

//builder.Services.AddTransient<IAccountService, AccountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
