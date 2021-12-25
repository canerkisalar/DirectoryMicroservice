using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Phonebook.Services.DataCapture.Consumers;
using Phonebook.Services.DataCapture.Infrastructure.Abstact;
using Phonebook.Services.DataCapture.Infrastructure.Concrete.EntityFramework;

namespace Phonebook.Services.DataCapture
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

            // Mass Transit 
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SnapPhonebookMessageCommandConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    cfg.ReceiveEndpoint("snap-phonebook", 
                        e =>
                    {
                        e.ConfigureConsumer<SnapPhonebookMessageCommandConsumer>(context);
                    });
                });
            });
            services.AddMassTransitHostedService();

           

            

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Directory.Services.DataCaptureService", Version = "v1" });
            });

            services.AddScoped<IPhonebookDal,EfPhonebookDal>();
            services.AddScoped<IContactDal, EfContactDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Directory.Services.DataCaptureService v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
