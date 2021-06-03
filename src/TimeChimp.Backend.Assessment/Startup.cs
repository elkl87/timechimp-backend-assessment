using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TimeChimp.Backend.Assessment.Mappings;
using TimeChimp.Backend.Assessment.Services;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace TimeChimp.Backend.Assessment
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApi(Configuration);
            services.AddServices();
            services.AddServiceProviders();
            services.Configure<FeedUrlsOptions>(Configuration.GetSection(FeedUrlsOptions.FeedUrl));
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Configuration["RedisCache:ConnectionString"];
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingFeed());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IFeedService, FeedService>();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseApi(Configuration, Environment);

            logger.LogInformation(default(EventId), $"{Assembly.GetExecutingAssembly().GetName().Name} started");
        }
    }
}
