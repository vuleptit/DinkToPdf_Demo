using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using DinkToPdf_Demo.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace DinkToPdf_Demo
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
            var context = new CustomAssemblyLoadContext();
            var architectureFolder = (IntPtr.Size == 8) ? "64bit" : "32bit";
            var wkHtmlToPdfPath = Path.Combine(Directory.GetCurrentDirectory(), architectureFolder, $"libwkhtmltox.dll");
            context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
            services.AddMvc()
                   .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                   .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
