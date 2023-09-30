
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
namespace signalRApiStudent.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddSwaggerExtensions(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory,"*.xml",SearchOption.TopDirectoryOnly).ToList();
                    foreach (string xmlFile in xmlFiles) 
                    {
                        options.IncludeXmlComments(xmlFile);
                    }
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version="v1",
                        Title="SignalR",
                        Description="This api is a exercise to learn SignalR",
                        Contact=new OpenApiContact
                        {
                            Name="Alexander Polanco",
                            Email="alexanderrpolanco11@gmail.com",
                            Url=new Uri("http://www.alexanderpolanco.com")
                        }
                    });
                    options.DescribeAllParametersInCamelCase();
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name="Authorization",
                        In=ParameterLocation.Header,
                        Type=SecuritySchemeType.ApiKey,
                        Scheme="Bearer",
                        BearerFormat="JWT",
                        Description="Input your Bearer token in this format: Bearer {your token here}"
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference=new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer",
                                },
                                Scheme="Bearer",
                                Name="Bearer",
                                In=ParameterLocation.Header
                            },new List<string>()
                        }, 
                    });
                });
        }
        public static void AddApiExtensions(this IServiceCollection services)
        {
            services.AddApiVersioning(

             confg =>
             {
                 confg.DefaultApiVersion = new ApiVersion(1, 0);
                 confg.AssumeDefaultVersionWhenUnspecified =true;
                 confg.ReportApiVersions =true;
             }
            );
        }
    }
}
