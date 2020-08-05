// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerHost.Quickstart.UI;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Serilog;
using Serilog.Sinks.File;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Environment.EnvironmentName = Configuration["environmentName"];
            bool useInMemoryStores = bool.Parse(Configuration["UseInMemoryStores"]);
            //var connectionString = Configuration.GetConnectionString("IdentityServerConnection");

            var identityServerDataDBConnectionString = Configuration.GetConnectionString("NameConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //if (useInMemoryStores)
                //{
                //    options.UseInMemoryDatabase("IdentityServerDb");
                //}
                //else
                //{
                    options.UseSqlServer(identityServerDataDBConnectionString);
                //}
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            ////Memory
            //var builder = services.AddIdentityServer()
            //.AddInMemoryIdentityResources(Config.Ids)
            //.AddInMemoryApiResources(Config.Apis)
            //.AddInMemoryClients(Config.Clients)
            //.AddTestUsers(TestUsers.Users);
            var builder = services.AddIdentityServer(
            //    options =>
            //{
            //    options.Events.RaiseErrorEvents = true;
            //    options.Events.RaiseInformationEvents = true;
            //    options.Events.RaiseFailureEvents = true;
            //    options.Events.RaiseSuccessEvents = true;
            //}
            );

            
            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
            //builder.AddSigningCredential(LoadCertificateFromStore());

            var migrationsAssembly = typeof(Startup)
                .GetTypeInfo().Assembly.GetName().Name;

            builder.AddConfigurationStore(options =>
            {
            options.ConfigureDbContext = builder =>
                builder.UseSqlServer(identityServerDataDBConnectionString
                , options => options.MigrationsAssembly(migrationsAssembly));
            });

            builder.AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(identityServerDataDBConnectionString,
                    options => options.MigrationsAssembly(migrationsAssembly));
            });

            builder.AddAspNetIdentity<ApplicationUser>();

            builder.AddProfileService<IdentityWithAdditionalClaimsProfileService>();

            //services.AddIdentityServerUserClaimsPrincipalFactory<ApplicationUser, IdentityRole>();

            services.AddAuthentication();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitializeDatabase(app);

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSerilogRequestLogging();
            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            //app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        public X509Certificate2 LoadCertificateFromStore()
        {
            string thumbPrint = "d99c43bc9715860f2d5a9b405d73721b29a14964";

            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint,
                    thumbPrint, true);
                if (certCollection.Count == 0)
                {
                    throw new Exception("The specified certificate wasn't found.");
                }
                return certCollection[0];
            }
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>().Database.Migrate();

                serviceScope.ServiceProvider
                    .GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider
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

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.ApiResourcs)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
