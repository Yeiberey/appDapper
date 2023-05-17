

using appDapper;
using appDapper2;
using System.Data;

namespace webApi
{
    public class Startup
    {
        private readonly string _MyCors = "";
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services )
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient<DbService>(_ =>  new DbService(Configuration.GetConnectionString("defaultConnection")));
            services.AddCors(options =>
            {
                options.AddPolicy(name: _MyCors, builder =>
                {
                    builder.AllowAnyOrigin()
                    /*builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "127.0.0.1")*/
                    .AllowAnyHeader()
                   .AllowAnyMethod();
                });
            });
            services.AddTransient<JWTService>();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(_MyCors);

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
