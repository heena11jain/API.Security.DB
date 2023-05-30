using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IDServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace IDServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRazorPages();
            string connectionString = Configuration.GetConnectionString("localdb"); 

            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;

            var env =Configuration.GetSection("ASPNETCORE_ENVIRONMENT");

            var identityServer = services.AddIdentityServer()
             .AddConfigurationStore(options =>
             {
                 options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
             });
             //.AddOperationalStore(options =>
             //{
             //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
             //    options.EnableTokenCleanup = true;
             //})
             //.AddCustomTokenRequestValidator<CustomTokenRequestValidator>();
             //.AddDeveloperSigningCredential();

            if (env.Value == "Development")
            {
                identityServer.AddDeveloperSigningCredential();
            }
            else
            {
                var certificate = new X509Certificate2(@"C:\My\certforProd.pfx", "test123");
                identityServer.AddSigningCredential(certificate);
            }
            //configure identity server4 in DI container
            //services.AddIdentityServer()
            //.AddInMemoryClients(Config.Clients)
            //.AddInMemoryIdentityResources(Config.IdentityResources)
            //.AddInMemoryApiResources(Config.ApiResources)
            //.AddInMemoryApiScopes(Config.ApiScopes)
            //.AddTestUsers(Config.TestUsers)
            //.AddDeveloperSigningCredential(); //IdentityServer needs certificates to verify it’s usage. But for development purposes and since we do not have any certificate with us, we use the AddDeveloperSigningCredential() extension. You can read more about it here.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                InitializeDatabase(app);
            }
            else if (env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                InitializeDatabase(app);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }

        static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope =
                    app.ApplicationServices
                        .GetService<IServiceScopeFactory>().CreateScope())
            {
                //serviceScope
                //    .ServiceProvider
                //        .GetRequiredService<PersistedGrantDbContext>()
                //        .Database.Migrate();

                var context =
                    serviceScope.ServiceProvider
                        .GetRequiredService<ConfigurationDbContext>();

                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                //if (!context.IdentityResources.Any())
                //{
                //    foreach (var resource in Config.IdentityResources)
                //    {
                //        context.IdentityResources.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}

                //if (!context.ApiResources.Any())
                //{
                //    foreach (var resource in Config.ApiResources)
                //    {
                //        context.ApiResources.Add(resource.ToEntity());
                //    }
                //    context.SaveChanges();
                //}
                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
