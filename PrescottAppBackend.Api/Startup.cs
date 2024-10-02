
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure DbContext and repositories
            services.AddDbContext<PrescottContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            
            // services.AddTransient<IUserService, UserService>();
            // services.AddTransient<IRoleService, RoleService>();

            // Configure Firebase Admin SDK
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("saapnetbook-firebase-adminsdk.json")
            });

            
            // Configure CORS for Specific Origin
            // services.AddCors(options =>
            // {
            //     options.AddPolicy("AllowSpecificOrigin",
            //         builder =>
            //         {
            //             builder.WithOrigins("http://localhost:4200") // Frontend URL
            //                 .AllowAnyHeader()
            //                 .AllowAnyMethod();
            //         });
            // });

            // Configure CORS for All Origin
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // Configure JWT authentication
            //ConfigureJwtAuthentication(services);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
        }

        private void ConfigureJwtAuthentication(IServiceCollection services)
        {
            // Configure JWT authentication logic here
            // Example:
            // var jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            // var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(options =>
            //     {
            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuerSigningKey = true,
            //             IssuerSigningKey = new SymmetricSecurityKey(key),
            //             ValidateIssuer = false,
            //             ValidateAudience = false,
            //             ValidateLifetime = true
            //         };
            //     });

            // services.AddSingleton(jwtSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SaapNet Book v1");
                    c.RoutePrefix = "api/";
                });
            }

            // Use the CORS policy for specific origin
            // app.UseCors("AllowSpecificOrigin");

            // Use the CORS policy for all origin
            app.UseCors("AllowAllOrigins");

            // Add custom middleware to the pipeline
            app.UseMiddleware<CustomMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
