using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            // Adiciona os serviços principais do IdentityServer4 ao contêiner de injeção de dependência. 
            services.AddIdentityServer()
                //Adiciona a configuração dos clientes do IdentityServer4 usando um armazenamento em memória
               .AddInMemoryClients(IdentityConfiguration.Clients)
                //Adiciona a configuração dos recursos de identidade (identity resources) usando um armazenamento em memória.
                .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
                //Adiciona a configuração dos recursos de API usando um armazenamento em memória
                .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
              //Adiciona a configuração dos escopos de API(API scopes) usando um armazenamento em memória
                .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                //Adiciona um conjunto de usuários de teste que podem ser usados para autenticação durante o desenvolvimento
                .AddTestUsers(IdentityConfiguration.TestUsers)
                //Adiciona uma credencial de assinatura para desenvolvimento. O IdentityServer4 precisa de uma chave para assinar os tokens que emite. Durante o desenvolvimento, você pode usar uma credencial de assinatura gerada automaticamente.
                //Em um ambiente de produção, você deve usar um certificado real para assinar os tokens de forma segura.
                .AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseIdentityServer();
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
