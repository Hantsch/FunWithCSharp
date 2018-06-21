using AspNetCoreWebMefDI.Framework;
using AspNetCoreWebMefDI.Interfaces;
using AspNetCoreWebMefDI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCoreWebMefDI
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mefServiceProvider = new MefServiceProvider();
            services.AddTransient<IControllerActivator, MefControllerActivator>(serviceProvider => new MefControllerActivator(mefServiceProvider, serviceProvider.GetService<ITypeActivatorCache>()));
            services.AddMvc();

            //services.AddScoped<IValueProvider, DictinaryValueProvider>();

            mefServiceProvider.DefaultServiceProvider = services.BuildServiceProvider();
            return mefServiceProvider;
            //return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
