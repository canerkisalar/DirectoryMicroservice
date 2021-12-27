using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Phonebook.Core.Repositories.MongoDb;
using Phonebook.Services.Phonebook.Consumers;
using Phonebook.Services.Phonebook.Infrastructure.Abstract;
using Phonebook.Services.Phonebook.Infrastructure.Concrete.MongoDb;
using Phonebook.Services.Phonebook.Services.Abstract;
using Phonebook.Services.Phonebook.Services.Concrete;
using Phonebook.Services.Phonebook.Settings.Abstract;
using Phonebook.Services.Phonebook.Settings.Concrete;

namespace Phonebook.Services.Phonebook
{

    [ExcludeFromCodeCoverage]
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
                x.AddConsumer<GiveAllPhonebooksMessageCommandConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                    cfg.ReceiveEndpoint("give-all-phonebooks", e =>
                     {
                         e.ConfigureConsumer<GiveAllPhonebooksMessageCommandConsumer>(context);
                     });
                });
            });
            services.AddMassTransitHostedService();

            // AutoMapper
            services.AddAutoMapper(typeof(Startup));
            // Additional Services
            services.AddScoped<IPhonebookService, PhonebookService>();
            services.AddScoped<IPhonebookDal>(dal => ActivatorUtilities.CreateInstance<MongoPhonebookDal>(dal, new MongoDbSettings()
            {
                Database = Configuration.GetSection("DatabaseSettings").GetSection("DatabaseName").Value,
                ConnectionString = Configuration.GetSection("DatabaseSettings").GetSection("ConnectionString").Value
            }));
            // options pattern 
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.ImplicitlyValidateChildProperties = true;
                fv.ImplicitlyValidateRootCollectionElements = true;
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                
            }); ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Directory.Services.Phonebook", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Directory.Services.Phonebook v1"));
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
