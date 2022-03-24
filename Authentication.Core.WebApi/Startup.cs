using Authentication.Core.WebApi.Handlers;
using Authentication.Core.WebApi.Models;
using Jose;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Core.WebApi
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication.Core.WebApi", Version = "v1" });
            });
            //JWT Settings 
            var _JWTSettings = Configuration.GetSection("JWTSettings");
            services.Configure<JWTSettings>(_JWTSettings);
            //DB Context
            services.AddDbContext<Auth_DBContext>(Options =>
            {
                //Options.UseSqlServer("Data Source=Jeberson-A-J\\SQLEXPRESS;Initial Catalog=api;Integrated Security=True;");
                Options.UseSqlServer("Data Source = 0.tcp.ngrok.io,17372; User Id = Jeberson_A_J; Password =Welcome@123; Initial Catalog = api; Integrated Security = False;");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction()) // env.IsProduction() is added to get the swagger UI in Production environment
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication.Core.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
