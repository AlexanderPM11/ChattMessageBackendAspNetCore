using crudSegnalR.Infrastructure.Identity;
using crudSegnalR.Infrastructure.Identity.Entities;
using crudSegnalR.Infrastructure.Identity.Seeds;
using crudSegnalR.Infrastructure.Shared;
using crudSignalR.Core.Application;
using crudSignalR.Infrastrucure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using signalRApiStudent.Extensions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using crudSegnalR.Infrastructure.httpclient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddLayerIdenityApi(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddLayerHttp(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddApiExtensions();
builder.Services.AddSwaggerExtensions();
builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication()
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = builder.Configuration.GetSection("FacebookAuth:AppId").ToString() ;
        facebookOptions.AppSecret = builder.Configuration.GetSection("FacebookAuth:AppSecret").ToString();
    });
var app = builder.Build();

// Configure Facebook authentication


// ...
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

}
//else
//{
//	app.UseExceptionHandler();
//	app.UseHsts();
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerExtensions();
app.UseHealthChecks("/health");

app.MapControllers();

app.MapHub<SiganalServer>("/studentHub");

 #region  creating seeds our application
using
(
  var scope= app.Services.CreateScope()
)
{
    var services= scope.ServiceProvider;
	try
	{
		var userManager = services.GetRequiredService<UserManager<ApplicantionUser>>();
		var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
		await DefaultRoles.SeedAsync(roleManager);
		await DefaultAdminUser.SeedAsync(userManager,roleManager);
		await DefaultBasicUser.SeedAsync(userManager, roleManager);
		await DefaultSuperAdminUser.SeedAsync(userManager, roleManager);
    }
	catch (Exception)
	{
		throw;
	}
}
#endregion


app.Run();
