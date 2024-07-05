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
            // Adiciona os servi�os principais do IdentityServer4 ao cont�iner de inje��o de depend�ncia. 
            services.AddIdentityServer()
                //Adiciona a configura��o dos clientes do IdentityServer4 usando um armazenamento em mem�ria
               .AddInMemoryClients(IdentityConfiguration.Clients)
                //Adiciona a configura��o dos recursos de identidade (identity resources) usando um armazenamento em mem�ria.
                .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
                //Adiciona a configura��o dos recursos de API usando um armazenamento em mem�ria
                .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
              //Adiciona a configura��o dos escopos de API(API scopes) usando um armazenamento em mem�ria
                .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                //Adiciona um conjunto de usu�rios de teste que podem ser usados para autentica��o durante o desenvolvimento
                .AddTestUsers(IdentityConfiguration.TestUsers)
                //Adiciona uma credencial de assinatura para desenvolvimento. O IdentityServer4 precisa de uma chave para assinar os tokens que emite. Durante o desenvolvimento, voc� pode usar uma credencial de assinatura gerada automaticamente.
                //Em um ambiente de produ��o, voc� deve usar um certificado real para assinar os tokens de forma segura.
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
