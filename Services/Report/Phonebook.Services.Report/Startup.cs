using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Phonebook.Services.Report.Consumers;
using Phonebook.Services.Report.Infrastructure.Abstract;
using Phonebook.Services.Report.Infrastructure.Abstract.Report;
using Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework;
using Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework.Report;
using Phonebook.Services.Report.Services.Abstract;
using Phonebook.Services.Report.Services.Concrete;

namespace Phonebook.Services.Report
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
                x.AddConsumer<GetAllPhonebooksMessageCommandConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                    cfg.ReceiveEndpoint("phonebook-alldata", e =>
                    {
                        e.ConfigureConsumer<GetAllPhonebooksMessageCommandConsumer>(context);
                    });

                });
            });
            services.AddMassTransitHostedService();
            //
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportHeadDal, EfReportHeadDal>();
            services.AddScoped<IReportItemDal, EfReportItemDal>();
            services.AddScoped<IPhonebookDal, EfPhonebookDal>();
            services.AddScoped<IContactDal, EfContactDal>();
            // AutoMapper
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Phonebook.Services.Report", Version = "v1" });
            });

          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var client = new Context())
            {
                client.Database.EnsureCreated();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Phonebook.Services.Report v1"));
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
