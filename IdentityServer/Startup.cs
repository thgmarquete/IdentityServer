using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //Configurar abaixo os servicos 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
            // Adiciona os serviços principais do IdentityServer4 ao contêiner de injeção de dependência. 
            var builder = services.AddIdentityServer()
                // Adiciona a configuração dos clientes do IdentityServer4 usando um armazenamento em memória
                .AddInMemoryClients(IdentityConfiguration.Clients)
                // Adiciona a configuração dos recursos de identidade (identity resources) usando um armazenamento em memória.
                .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
                // Adiciona a configuração dos recursos de API usando um armazenamento em memória
                .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
                // Adiciona a configuração dos escopos de API(API scopes) usando um armazenamento em memória
                .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                // Adiciona um conjunto de usuários de teste que podem ser usados para autenticação durante o desenvolvimento
                .AddTestUsers(IdentityConfiguration.TestUsers)
                // Adiciona uma credencial de assinatura para desenvolvimento. O IdentityServer4 precisa de uma chave para assinar os tokens que emite.
                .AddDeveloperSigningCredential();

            // Configurar logging para o IdentityServer
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseIdentityServer();
            app.Use(async (context, next) =>
            {
                logger.LogInformation("Handling request: " + context.Request.Method + " " + context.Request.Path);
                await next.Invoke();
                logger.LogInformation("Finished handling request.");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
