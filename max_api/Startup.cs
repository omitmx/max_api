using max_api.DAL;
using max_api.Models;
using max_api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace max_api
{
    public class Startup
    {
        readonly string MiCors = "MiCors";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(op =>
            //{
            //    op.AddPolicy(name: MiCors, builder =>
            //    {
            //        builder.WithOrigins("*");
            //        builder.AllowAnyHeader();
            //        builder.AllowAnyMethod();
            //        //builder.AllowCredentials();
            //      //  builder.WithOrigins("https://localhost:44361/")
            //      //.AllowAnyHeader()
            //      //.AllowAnyMethod()
            //      //.AllowCredentials();
            //});



            //});
            services.AddCors(options =>
            {
                options.AddPolicy(MiCors,
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials());
            });


            services.AddControllers();

            //SMTP 
            var smtp = Configuration.GetSection("SmtpNoti");
            services.Configure<SmtpNoti>(smtp);
            services.AddSingleton<IEnvioCorreo, cEnvioCorreo>();
            services.AddDbContext<DbmaxContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(MiCors);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
