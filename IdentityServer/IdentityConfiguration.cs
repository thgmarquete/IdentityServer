using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
public class IdentityConfiguration
{
    //Criando usuario com as claims que são informações sobre o usuario que são transimitidas em diversas partes do sismtemas 
    //Representam atributos ou propriedades de um usuario, transportam as informações de maneira segura em token
    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
                new TestUser
                {
                     SubjectId = "1144",
                     Username = "TMarquete",
                     Password = "skate",
                     Claims =
                     {
                        new Claim(JwtClaimTypes.Name, "Marquete .Net"),
                        new Claim(JwtClaimTypes.GivenName, "Marquete"),
                        new Claim(JwtClaimTypes.FamilyName, ".Net"),
                        new Claim(JwtClaimTypes.WebSite, "http://marquete.net"),
                     }
              }
    };
    //Aqui estamos definindo o recurso usado, os recursos de identidade padrao são userId, email, telefone
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
               new IdentityResources.OpenId(),
               new IdentityResources.Profile(),
        };
    //Aqui definimos o escopo da API como leitura e escritae damos o nome de myAPI
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                new ApiScope("myApi.read"),
                new ApiScope("myApi.write"),
        };
    //Abaixo estamos definindo os recursos da API e passando um hash para ela que sera salvo automaticamente 
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
               new ApiResource("myApi")
               {
                   Scopes = new List<string>{ "myApi.read","myApi.write" },
                   ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
               }
        };
    //Definindo aqui abaixo o perfil de usuario que tera acesso a nossa API e definindo GrantTypes que significa que tera acesso a 
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
                new Client
                {
                    ClientId = "cwm.client",
                    ClientName = "Credencias de acesso",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "myApi.read" }
                    
                },
        };
}
