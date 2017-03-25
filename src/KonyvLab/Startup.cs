using KonyvLab.dal.Managers;
using KonyvLab.dal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KonyvLab
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityWithMongoStoresUsingCustomTypes<ApplicationUser, IdentityRole>(Configuration.GetConnectionString("DefaultConnection"))
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
            });


            services.AddTransient<ReviewManager>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Errors");
                app.UseStatusCodePagesWithReExecute("Errors/Error/{0}");
            }

            app.UseIdentity();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            /*
             * kliens + szerveroldali validálás
             * ui
             * dal, üzleti logika a manager osztályokba
             * bootstarp
             * fukció bővítés
             * keresés felhasználóra
             * kép feltöltés
             */

            /*
             * doksi
             * külön userhez tartozó veiw cshtml-ből vagy controllerből
             * mongodb tábla join szerű működés
             * CRUD műveletek post/get/put/delete vagy elég get/post
             * solution mappa szerkezet web->dal
             * bootstrap
             * validálás inkább kliens/szerver oldalon
             * IOC db elérésre szolgáló osztályt scoped/transient?
             */

        }
    }
}
