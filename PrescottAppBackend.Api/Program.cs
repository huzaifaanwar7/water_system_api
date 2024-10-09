using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.OpenApi.Models;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;
using PrescottAppBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
// Configure Firebase Admin SDK
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("prescott-firebase-adminsdk.json")
});

// Register the DbContext with a connection string
// builder.Services.AddDbContext<PrescottContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the DbContext with a connection string (for MySQL)
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=127.0.0.1;Database=Prescott;User=root;Password=;";
builder.Services.AddDbContext<PrescottContext>(options =>
   options.UseMySQL(connectionString));

// Configure CORS for All Origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PRESCOTT API", Version = "v1" });
        });

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IDDLService, DDLService>();
builder.Services.AddTransient<IBuildingService, BuildingService>();

builder.Services.AddControllers();

// Configure the application services and middleware by using the Startup class
var app = builder.Build();
var env = app.Services.GetRequiredService<IWebHostEnvironment>();

//if (env.IsDevelopment())
//{
    app.MapSwagger();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SaapNet Book v1");
    });
//}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// Add custom middleware to the pipeline
app.UseMiddleware<CustomMiddleware>();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });





// Run the configured pipeline
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
