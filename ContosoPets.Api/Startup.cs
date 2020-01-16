using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoPets.DataAccess.Data;
using ContosoPets.DataAccess.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ContosoPets.Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddScoped<OrderService> ();

            var builder = new SqlConnectionStringBuilder (
                Configuration.GetConnectionString ("ContosoPets"));
            //IConfigurationSection contosoPetsCredentials =
            //Configuration.GetSection("ContosoPetsCredentials");

            builder.UserID = "sa"; //contosoPetsCredentials["UserId"];
            builder.Password = "123"; //contosoPetsCredentials["Password"];

            services.AddDbContext<ContosoPetsContext> (options =>
                options.UseSqlServer (builder.ConnectionString)
                .EnableSensitiveDataLogging (Configuration.GetValue<bool> ("Logging:EnableSqlParameterLogging")));

            services.AddControllers ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}