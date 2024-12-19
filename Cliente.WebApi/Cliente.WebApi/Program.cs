using Cliente.WebApi.Modules.Authentication;
using Cliente.WebApi.Modules.Feature;
using Cliente.WebApi.Modules.Injection;
using Cliente.WebApi.Modules.Mapper;
using Cliente.WebApi.Modules.Swagger;
using Cliente.WebApi.Modules.Validator;
using Cliente.WebApi.Modules.Versioning;
using Cliente.WebApi.Modules.WatchDog;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WatchDog;

namespace Cliente.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            }
            else
            {
                builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            }


            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new DefaultNamingStrategy() // Asegura la utilización de Pascal Case
                    };
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddVersioning();
            builder.Services.AddAuthentication(builder.Configuration);
            builder.Services.AddValidator();
            builder.Services.AddMapper();
            builder.Services.AddFeature(builder.Configuration);
            builder.Services.AddInjection(builder.Configuration);
            builder.Services.AddSwaggerDocumentation();
            builder.Services.AddWatchDog(builder.Configuration);


            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (DbUpdateException ex)
                {
                    context.Response.StatusCode = 400;
                    var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() } };
                    var result = JsonConvert.SerializeObject(new { Mensaje = ex.Message }, settings);
                    await context.Response.WriteAsync(result);
                }

                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() } };
                    var result = JsonConvert.SerializeObject(new { Mensaje = $"Ah ocurrido un error inesperado en el servidor, por favor contactar al administrador del sistema. ({ex.Message})" }, settings);

                    await context.Response.WriteAsync(result);
                }

                if (context.Response.StatusCode == 401)
                {
                    var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() } };
                    var result = JsonConvert.SerializeObject(new { Mensaje = "No se ha autenticado para realizar este proceso." }, settings);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }
                else if (context.Response.StatusCode == 403)
                {
                    var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() } };
                    var result = JsonConvert.SerializeObject(new { Mensaje = "No tienes permiso para realizar esta acción." }, settings);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }
            });


            app.UseWatchDogExceptionLogger();
            app.UseAuthorization();
            app.UseCors("policyApi");

            app.MapControllers();
            app.UseWatchDog(opt =>
            {
                opt.WatchPageUsername = builder.Configuration["WatchDog:WatchPageUsername"];
                opt.WatchPagePassword = builder.Configuration["WatchDog:WatchPagePassword"];
            });
            app.Run();
        }
    }
}
