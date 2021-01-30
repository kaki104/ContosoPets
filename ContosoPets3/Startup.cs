using ContosoPets3.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContosoPets3
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
            services.AddRazorPages();
            services.AddDbContextPool<ContosoPetsContext>(options =>
                options
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=ContosoPets.db"));
            //.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContosoPets;Integrated Security=true"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                IEnumerable<string> assemblyMigrations = scope.ServiceProvider.GetService<ContosoPetsContext>().Database.GetMigrations();
                IEnumerable<string> appliedMigrations = scope.ServiceProvider.GetService<ContosoPetsContext>().Database.GetAppliedMigrations();
                IEnumerable<string> pendingMigrations = scope.ServiceProvider.GetService<ContosoPetsContext>().Database.GetPendingMigrations();

                //Check that all applied migrations exist in the assembly
                if (appliedMigrations.Any(a => !assemblyMigrations.Contains(a)))
                {
                    throw new Exception("There are applied migrations that do not exist in this assembly. All applied migrations must exist in the assembly. Aborting the migration.");
                }

                // Check that pending migrations will be applied chronologically after applied migrations
                IEnumerable<ulong> appliedTimestamps = appliedMigrations.Select(m => Convert.ToUInt64(m.Substring(0, 14)));
                IEnumerable<ulong> pendingTimestamps = pendingMigrations.Select(m => Convert.ToUInt64(m.Substring(0, 14)));
                if (appliedTimestamps.Any(a => pendingTimestamps.Any(p => a > p)))
                {
                    throw new Exception("There are pending migrations with a timestamp earlier than the timestamps of one or more applied migrations. Migrations must be applied chronologically. Aborting the migration.");
                }

                if (pendingMigrations.Any())
                {
                    scope.ServiceProvider.GetService<ContosoPetsContext>().Database.Migrate();
                }
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
